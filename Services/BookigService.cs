
using BookingSystem.Data;
using BookingSystem.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Services
{
    public class BookingService
    {
        private readonly BookingDbContext _db;
        public BookingService(BookingDbContext db) => _db = db;

        public async Task<Booking?> BookClassAsync(int userId, int classId)
        {
            var schedule = await _db.ClassSchedules.FindAsync(classId);
            if (schedule == null) return null;

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
    }
}