namespace BookingSystem.Models
{
    public record UserDto(int Id, string Email, bool EmailVerified);
    public record RegisterRequest(string Email, string Password);

    public record ChangePasswordRequest(int UserId, string CurrentPassword, string NewPassword);
    public record ResetPasswordRequest(string Email, string NewPassword);
}