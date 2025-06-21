using BookingSystem.Data;
using BookingSystem.Entities;
using BookingSystem.Services;
using BookingSystem.Mocks;
using BookingSystem.Cache;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;
using Hangfire;
using Hangfire.MemoryStorage;
using BookingSystem.Schedulers;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

var odataBuilder = new ODataConventionModelBuilder();
odataBuilder.EntitySet<User>("Users");
odataBuilder.EntitySet<Package>("Packages");
odataBuilder.EntitySet<ClassSchedule>("ClassSchedules");
odataBuilder.EntitySet<Booking>("Bookings");

builder.Services.AddControllers().AddOData(opt =>
    opt.AddRouteComponents("odata", odataBuilder.GetEdmModel())
       .Select().Filter().OrderBy().Expand().Count());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen();
builder.Services.AddHangfire(config => config.UseMemoryStorage());
builder.Services.AddHangfireServer();

if (!string.IsNullOrWhiteSpace(connectionString))
{
    builder.Services.AddDbContext<BookingDbContext>(opt =>
        opt.UseMySQL(connectionString));
    Console.WriteLine("Using MySQL: " + connectionString);
}
else
{
    builder.Services.AddDbContext<BookingDbContext>(opt =>
        opt.UseInMemoryDatabase("booking"));
    Console.WriteLine("Using InMemory DB");
}


builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<PackageService>();
builder.Services.AddScoped<ClassService>();
builder.Services.AddScoped<BookingService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddSingleton<FirebaseAuthService>();
builder.Services.AddSingleton<RedisCacheHelper>();
builder.Services.AddScoped<BookingJobService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseHangfireDashboard();
app.MapControllers();

app.Run();