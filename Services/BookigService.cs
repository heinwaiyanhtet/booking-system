using BookingSystem.Data;
using BookingSystem.Entities;
using BookingSystem.Cache;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Services
{
    public class BookingService
    {
        private readonly BookingDbContext _db;
        private readonly RedisCacheHelper _cache;
        public BookingService(BookingDbContext db, RedisCacheHelper cache)
        {
            _db = db;
            _cache = cache;
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
                var currentCount = await _db.Bookings
                    .Where(b => b.ClassScheduleId == classId && !b.Canceled)
                    .CountAsync();

                if (currentCount >= schedule.Capacity)
                {
                    var existing = await _db.Waitlists.FirstOrDefaultAsync(w => w.UserId == userId && w.ClassScheduleId == classId);
                    if (existing == null)
                    {
                        _db.Waitlists.Add(new Waitlist
                        {
                            UserId = userId,
                            ClassScheduleId = classId,
                            AddedAt = DateTime.UtcNow
                        });
                        await _db.SaveChangesAsync();
                    }
                    return null;
                }

                var overlap = await _db.Bookings
                    .Include(b => b.ClassSchedule)
                    .AnyAsync(b => b.UserId == userId && !b.Canceled && b.ClassScheduleId != classId &&
                                   b.ClassSchedule!.StartTime == schedule.StartTime);
                if (overlap) return null;

                var booking = new Booking
                {
                    UserId = userId,
                    ClassScheduleId = classId,
                    BookedAt = DateTime.UtcNow
                };
                _db.Bookings.Add(booking);
                await _db.SaveChangesAsync();
                return booking;
            }
            finally
            {
                await _cache.ReleaseLockAsync(lockKey, lockValue);
            }
        }

        public async Task<bool> CancelAsync(int bookingId)
        {
            var booking = await _db.Bookings.FindAsync(bookingId);
            if (booking == null || booking.Canceled) return false;
            booking.Canceled = true;
            await _db.SaveChangesAsync();
            await PromoteWaitlistAsync(booking.ClassScheduleId);
            return true;
        }

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