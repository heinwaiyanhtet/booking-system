using BookingSystem.Data;
using BookingSystem.Entities;
using BookingSystem.Services;
using BookingSystem.Cache;
using BookingSystem.Schedulers;
using Microsoft.EntityFrameworkCore;
using Hangfire;
using Microsoft.Extensions.Configuration;
using System;

namespace BookingSystem.Tests;

public class BookingServiceTests
{
    private BookingDbContext CreateDb()
    {
        var options = new DbContextOptionsBuilder<BookingDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new BookingDbContext(options);
    }

    [Fact]
    public async Task BookClassAsync_CreatesBooking()
    {
        using var db = CreateDb();
        var cache = new RedisCacheHelper(new ConfigurationBuilder().AddInMemoryCollection().Build());
        JobStorage.Current = new Hangfire.MemoryStorage.MemoryStorage();
        var jobSvc = new BookingJobService(db, new BackgroundJobClient());
        var service = new BookingService(db, cache, jobSvc);

        var user = new User { Email = "a@b.com", PasswordHash = "x" };
        db.Users.Add(user);

        var pkg = new Package { Name = "P1", Country = "US", Credits = 10, Price = 10, ExpireAt = DateTime.UtcNow.AddDays(1) };
        db.Packages.Add(pkg);

        var up = new UserPackage { User = user, Package = pkg, RemainingCredits = 10, PurchasedAt = DateTime.UtcNow };
        db.UserPackages.Add(up);
        
        var schedule = new ClassSchedule { Title = "Yoga", Country = "US", RequiredCredits = 5, StartTime = DateTime.UtcNow.AddHours(5), Capacity = 10 };
        db.ClassSchedules.Add(schedule);
        await db.SaveChangesAsync();

        var booking = await service.BookClassAsync(user.Id, schedule.Id);
        Assert.NotNull(booking);
        Assert.Equal(booking!.UserId, user.Id);
        Assert.Equal(booking.ClassScheduleId, schedule.Id);
    }

    [Fact]
    public async Task CancelAsync_RefundsCredits()
    {
        using var db = CreateDb();
        var cache = new RedisCacheHelper(new ConfigurationBuilder().AddInMemoryCollection().Build());
        JobStorage.Current = new Hangfire.MemoryStorage.MemoryStorage();
        var jobSvc = new BookingJobService(db, new BackgroundJobClient());
        var service = new BookingService(db, cache, jobSvc);

        var user = new User { Email = "a@b.com", PasswordHash = "x" };
        db.Users.Add(user);
        var pkg = new Package { Name = "P1", Country = "US", Credits = 10, Price = 10, ExpireAt = DateTime.UtcNow.AddDays(1) };
        db.Packages.Add(pkg);
        var up = new UserPackage { User = user, Package = pkg, RemainingCredits = 5, PurchasedAt = DateTime.UtcNow };
        db.UserPackages.Add(up);
        var schedule = new ClassSchedule { Title = "Yoga", Country = "US", RequiredCredits = 5, StartTime = DateTime.UtcNow.AddHours(5), Capacity = 10 };
        db.ClassSchedules.Add(schedule);
        var booking = new Booking { User = user, ClassSchedule = schedule, UserPackage = up, BookedAt = DateTime.UtcNow };
        db.Bookings.Add(booking);
        await db.SaveChangesAsync();

        var result = await service.CancelAsync(booking.Id);
        Assert.True(result);
        var updated = await db.UserPackages.FindAsync(up.Id);
        Assert.Equal(10, updated!.RemainingCredits); // refunded
    }
}