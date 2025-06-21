using BookingSystem.Data;
using BookingSystem.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Services
{
    public class PackageService
    {
        private readonly BookingDbContext _db;
        public PackageService(BookingDbContext db) => _db = db;


        public async Task<List<Package>> GetAllPackagesAsync()
        {
            return await _db.Packages.ToListAsync();
        }


        public async Task<List<Package>> GetPackagesAsync(string country)
        {
            return await _db.Packages.Where(p => p.Country == country).ToListAsync();
        }
    }
}