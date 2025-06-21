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
    }
}