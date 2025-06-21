
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingSystem.Entities
{
    public class UserPackage
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public User? User { get; set; }
        
        [ForeignKey("Package")]
        public int PackageId { get; set; }
        public Package? Package { get; set; }
        public int RemainingCredits { get; set; }
        public DateTime PurchasedAt { get; set; }
    }
}
