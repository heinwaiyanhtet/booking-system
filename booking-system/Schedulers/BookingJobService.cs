using BookingSystem.Data;
using BookingSystem.Entities;
using Hangfire;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Schedulers
{
    public class BookingJobService
    {
        private readonly BookingDbContext _db;
        private readonly IBackgroundJobClient _jobs;
        public BookingJobService(BookingDbContext db, IBackgroundJobClient jobs)
        {
            _db = db;
            _jobs = jobs;
        }

        public void ScheduleRefundForWaitlist(int classId, DateTime runAt)
        {
            _jobs.Schedule<BookingJobService>(s => s.RefundWaitlistAsync(classId), runAt);
        }

        public async Task RefundWaitlistAsync(int classId)
        {
            var waits = await _db.Waitlists.Where(w => w.ClassScheduleId == classId).ToListAsync();
            if (waits.Count == 0) return;
            foreach (var wait in waits)
            {
                var package = await _db.UserPackages.FindAsync(wait.UserPackageId);
                if (package != null)
                {
                    package.RemainingCredits += wait.ReservedCredits;
                }
                _db.Waitlists.Remove(wait);
            }
            await _db.SaveChangesAsync();
        }
    }
}