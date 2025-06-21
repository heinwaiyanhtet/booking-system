namespace BookingSystem.Models
{
    public record LoginRequest(string Email, string Password);
    public record LoginResponse(string Token);
    public record FirebaseLoginRequest(string IdToken);
}