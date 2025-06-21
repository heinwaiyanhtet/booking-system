namespace BookingSystem.Models
{
    public record UserPackageDto(int PackageId, string Name, bool Expired, int RemainingCredits);
}