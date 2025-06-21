namespace BookingSystem.Models
{
    public record UserDto(int Id, string Email, bool EmailVerified);
    public record RegisterRequest(string Email, string Password);
}