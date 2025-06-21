
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingSystem.Entities
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User? User { get; set; }
        [ForeignKey("ClassSchedule")]
        public int ClassScheduleId { get; set; }
        public ClassSchedule? ClassSchedule { get; set; }
        public bool Canceled { get; set; }
        public DateTime BookedAt { get; set; }
         public bool CheckedIn { get; set; }
    }
}