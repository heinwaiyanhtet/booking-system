using BookingSystem.Data;
using BookingSystem.Services;
using BookingSystem.Mocks;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Tests;

public class UserServiceTests
{
    private BookingDbContext CreateDb()
    {
        var options = new DbContextOptionsBuilder<BookingDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new BookingDbContext(options);
    }

    [Fact]
    public async Task RegisterAsync_SavesUser()
    {
        using var db = CreateDb();
        var emailMock = new EmailMockService();
        var service = new UserService(db, emailMock);

        var user = await service.RegisterAsync("test@example.com", "pass123");

        var saved = await db.Users.FindAsync(user.Id);
        Assert.NotNull(saved);
        Assert.NotEmpty(saved!.PasswordHash);
    }
}