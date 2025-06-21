namespace BookingSystem.Models
{
    public record ClassDto(int Id, string Title, string Country, int RequiredCredits, DateTime StartTime, int Capacity);
}