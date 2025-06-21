using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingSystem.Entities
{
    public class Waitlist
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User? User { get; set; }
        [ForeignKey("ClassSchedule")]
        public int ClassScheduleId { get; set; }
        public ClassSchedule? ClassSchedule { get; set; }
        public DateTime AddedAt { get; set; }

        [ForeignKey("UserPackage")]
        public int UserPackageId { get; set; }
        public UserPackage? UserPackage { get; set; }
        public int ReservedCredits { get; set; }


        
    }
}