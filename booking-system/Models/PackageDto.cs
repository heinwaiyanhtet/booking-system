namespace BookingSystem.Models
{
    public record PackageDto(int Id, string Name, string Country, int Credits, decimal Price, DateTime ExpireAt);
}