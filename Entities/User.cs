using System.ComponentModel.DataAnnotations;

namespace BookingSystem.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string? ProfilePicture { get; set; }
        public string? UserName { get; set; }

        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        public bool EmailVerified { get; set; }

    }
}