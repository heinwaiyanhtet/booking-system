namespace BookingSystem.Entities
{
    public class ClassSchedule
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Country { get; set; } = string.Empty;
        [Required]
        public int RequiredCredits { get; set; }
        public DateTime StartTime { get; set; }
        public int Capacity { get; set; }
    }
}