using Microsoft.EntityFrameworkCore;
using BookingSystem.Entities;

namespace BookingSystem.Data
{
    public class BookingDbContext : DbContext
    {
        public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options) { }
        public DbSet<User> Users => Set<User>();
        public DbSet<Package> Packages => Set<Package>();
        public DbSet<UserPackage> UserPackages => Set<UserPackage>();
        public DbSet<ClassSchedule> ClassSchedules => Set<ClassSchedule>();
        public DbSet<Booking> Bookings => Set<Booking>();
        public DbSet<Waitlist> Waitlists => Set<Waitlist>();
    }
}