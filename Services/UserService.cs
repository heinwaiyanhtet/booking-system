
using BookingSystem.Data;
using BookingSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace BookingSystem.Services
{
    public class UserService
    {
        private readonly BookingDbContext _db;
        private readonly PasswordHasher<User> _hasher = new();
        private readonly Mocks.EmailMockService _emailMock;
        public UserService(BookingDbContext db, Mocks.EmailMockService emailMock)
        {
            _db = db;
            _emailMock = emailMock;
        }

        public async Task<User> RegisterAsync(string email, string password)
        {
            var user = new User { Email = email };
            user.PasswordHash = _hasher.HashPassword(user, password);
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            _emailMock.SendVerifyEmail(user.Email);
            return user;
        }

        public async Task<User?> AuthenticateAsync(string email, string password)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return null;
            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return result == PasswordVerificationResult.Success ? user : null;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _db.Users.FindAsync(id);
        }

        public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var user = await _db.Users.FindAsync(userId);
            if (user == null) return false;
            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, currentPassword);
            if (result != PasswordVerificationResult.Success) return false;
            user.PasswordHash = _hasher.HashPassword(user, newPassword);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ResetPasswordAsync(string email, string newPassword)
        {
            var user = await GetByEmailAsync(email);
            if (user == null) return false;
            user.PasswordHash = _hasher.HashPassword(user, newPassword);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> VerifyEmailAsync(int userId)
        {
            var user = await _db.Users.FindAsync(userId);
            if (user == null) return false;
            user.EmailVerified = true;
            await _db.SaveChangesAsync();
            return true;
        }
    }
}