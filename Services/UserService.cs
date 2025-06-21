using BookingSystem.Data;
using BookingSystem.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Services
{
    public class UserService
    {
        private readonly BookingDbContext _db;
        public UserService(BookingDbContext db) => _db = db;

        public async Task<User> RegisterAsync(string email, string password)
        {
            var user = new User { Email = email, PasswordHash = password }; // TODO: hash password
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return user;
        }

        public async Task<User?> AuthenticateAsync(string email, string password)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Email == email && u.PasswordHash == password);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}