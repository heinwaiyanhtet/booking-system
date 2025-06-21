namespace BookingSystem.Models
{
    public record BookingDto(int Id, int ClassScheduleId, bool Canceled, DateTime BookedAt);
}