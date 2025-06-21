using BookingSystem.Data;
using BookingSystem.Entities;
using BookingSystem.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

var odataBuilder = new ODataConventionModelBuilder();
odataBuilder.EntitySet<User>("Users");
odataBuilder.EntitySet<Package>("Packages");
odataBuilder.EntitySet<ClassSchedule>("ClassSchedules");
odataBuilder.EntitySet<Booking>("Bookings");

// Add services
builder.Services.AddControllers().AddOData(opt =>
    opt.AddRouteComponents("odata", odataBuilder.GetEdmModel())
       .Select().Filter().OrderBy().Expand().Count());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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


// Register services
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<PackageService>();
builder.Services.AddScoped<ClassService>();
builder.Services.AddScoped<BookingService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddSingleton<FirebaseAuthService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();