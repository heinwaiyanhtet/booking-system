using BookingSystem.Data;
using BookingSystem.Entities;
using BookingSystem.Cache;
using BookingSystem.Schedulers;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Services
{
    public class BookingService
    {
        private readonly BookingDbContext _db;
        private readonly RedisCacheHelper _cache;
        private readonly BookingJobService _jobs;
        public BookingService(BookingDbContext db, RedisCacheHelper cache, BookingJobService jobs)
        {
            _db = db;
            _cache = cache;
            _jobs = jobs;
        }

        public async Task<Booking?> BookClassAsync(int userId, int classId)
        {
            var schedule = await _db.ClassSchedules.FindAsync(classId);


            if (schedule == null) return null;

            var lockKey = $"booking_lock_{classId}";
            var lockValue = Guid.NewGuid().ToString();
            if (!await _cache.AcquireLockAsync(lockKey, lockValue, TimeSpan.FromSeconds(5)))
            {
                return null;
            }

            try
            {
                var countKey = $"booking_count_{classId}";
                var cachedCount = await _cache.GetIntAsync(countKey);
                if (cachedCount == null)
                {
                    cachedCount = await _db.Bookings
                        .Where(b => b.ClassScheduleId == classId && !b.Canceled)
                        .CountAsync();
                    await _cache.SetIntAsync(countKey, cachedCount.Value);
                }

                if (cachedCount >= schedule.Capacity)
                {

                    var existing = await _db.Waitlists.FirstOrDefaultAsync(w => w.UserId == userId && w.ClassScheduleId == classId);

                    if (existing == null)
                    {

                        // each package is available to only one country.
                                    Console.WriteLine(schedule);

                        var pkg = await _db.UserPackages
                        .Include(p => p.Package)
                        .FirstOrDefaultAsync(p => p.UserId == userId
                            && p.Package!.Country == schedule.Country
                            && p.RemainingCredits >= schedule.RequiredCredits);


                        if (pkg == null) return null;
                        pkg.RemainingCredits -= schedule.RequiredCredits;

                        var wait = new Waitlist
                        {
                            UserId = userId,
                            ClassScheduleId = classId,
                            UserPackageId = pkg.Id,
                            ReservedCredits = schedule.RequiredCredits,
                            AddedAt = DateTime.UtcNow
                        };

                        _db.Waitlists.Add(wait);
                        await _db.SaveChangesAsync();
                        _jobs.ScheduleRefundForWaitlist(classId, schedule.StartTime);
                    }
                    return null;
                }

                // class တစ်ခု၏ ခွင့်ပြုသော အရေအတွက် ထက်များခွင့်မပြုပါ 
                
                var newCount = await _cache.IncrementAsync(countKey);
                
                if (newCount > schedule.Capacity)
                {
                    await _cache.DecrementAsync(countKey);

                    var existing = await _db.Waitlists.FirstOrDefaultAsync(w => w.UserId == userId && w.ClassScheduleId == classId);

                    if (existing == null)
                    {
                        // each package is available to only one country.
                        Console.WriteLine(schedule);

                        var pkg = await _db.UserPackages
                            .Include(p => p.Package)
                            .FirstOrDefaultAsync(
                                p => p.UserId == userId
                                && p.Package!.Country == schedule.Country
                                && p.RemainingCredits >= schedule.RequiredCredits);


                        if (pkg == null) return null;

                        pkg.RemainingCredits -= schedule.RequiredCredits;
                        var wait = new Waitlist
                        {
                            UserId = userId,
                            ClassScheduleId = classId,
                            UserPackageId = pkg.Id,
                            ReservedCredits = schedule.RequiredCredits,
                            AddedAt = DateTime.UtcNow
                        };
                        _db.Waitlists.Add(wait);
                        await _db.SaveChangesAsync();
                        _jobs.ScheduleRefundForWaitlist(classId, schedule.StartTime);
                    }
                    return null;
                }

                var overlap = await _db.Bookings
                    .Include(b => b.ClassSchedule)
                    .AnyAsync(b => b.UserId == userId && !b.Canceled && b.ClassScheduleId != classId &&
                                   b.ClassSchedule!.StartTime == schedule.StartTime);

                if (overlap) return null;


                // each package is available to only one country.

                 Console.WriteLine(schedule);

                  var userPkg = await _db.UserPackages
                    .Include(p => p.Package)
                    .FirstOrDefaultAsync(p => p.UserId == userId
                        && p.Package!.Country == schedule.Country
                        && p.RemainingCredits >= schedule.RequiredCredits);
                        

                    if (userPkg == null) return null;
                userPkg.RemainingCredits -= schedule.RequiredCredits;
                var booking = new Booking
                {
                    UserId = userId,
                    ClassScheduleId = classId,
                     UserPackageId = userPkg.Id,
                    BookedAt = DateTime.UtcNow
                };
                _db.Bookings.Add(booking);
                await _db.SaveChangesAsync();
                return booking;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
            finally
            {
                await _cache.ReleaseLockAsync(lockKey, lockValue);
            }
        }

        public async Task<bool> CancelAsync(int bookingId)
        {
            var booking =  _db.Bookings.FirstOrDefault(b => b.Id == bookingId);
            if (booking == null || booking.Canceled) return false;
            booking.Canceled = true;

            var schedule = await _db.ClassSchedules.FindAsync(booking.ClassScheduleId);

            if (schedule != null)
            {
                var timeToClass = schedule.StartTime - DateTime.UtcNow;
                if (timeToClass >= TimeSpan.FromHours(4))
                {
                    if (booking.UserPackageId != null)
                    {
                        var pkg = await _db.UserPackages.FindAsync(booking.UserPackageId);
                        if (pkg != null)
                        {
                            pkg.RemainingCredits += schedule.RequiredCredits;
                        }
                    }
                }
            }



            await _db.SaveChangesAsync();
            await PromoteWaitlistAsync(booking.ClassScheduleId);
            return true;
        }

        // If someone from class booked cancel the class, add a waitlist user as booked
        // as (FIFO from waitlist).
        private async Task PromoteWaitlistAsync(int classId)
        {
            var schedule = await _db.ClassSchedules.FindAsync(classId);

            if (schedule == null) return;

            var current = await _db.Bookings.Where(b => b.ClassScheduleId == classId && !b.Canceled).CountAsync();
            if (current >= schedule.Capacity) return;

            var wait = await _db.Waitlists.Where(w => w.ClassScheduleId == classId)
                .OrderBy(w => w.AddedAt).FirstOrDefaultAsync();
                
            if (wait != null)
            {
                _db.Waitlists.Remove(wait);
                _db.Bookings.Add(new Booking
                {
                    UserId = wait.UserId,
                    ClassScheduleId = classId,
                    UserPackageId = wait.UserPackageId,
                    BookedAt = DateTime.UtcNow
                });
                await _db.SaveChangesAsync();
            }
        }

        public async Task<bool> CheckInAsync(int bookingId)
        {
            var booking = await _db.Bookings.FindAsync(bookingId);
            if (booking == null || booking.Canceled) return false;
            booking.CheckedIn = true;
            await _db.SaveChangesAsync();
            return true;
        }
    }
}