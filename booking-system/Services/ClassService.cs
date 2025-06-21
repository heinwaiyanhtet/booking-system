using BookingSystem.Data;
using BookingSystem.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Services
{
    public class ClassService
    {
        private readonly BookingDbContext _db;
        public ClassService(BookingDbContext db) => _db = db;

        public async Task<List<ClassSchedule>> GetSchedulesAsync(string country)
        {
            return await _db.ClassSchedules.Where(c => c.Country == country).ToListAsync();
        }
    }
}