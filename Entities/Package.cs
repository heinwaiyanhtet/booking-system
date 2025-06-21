namespace BookingSystem.Entities
{
    public class Package
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Country { get; set; } = string.Empty;
        [Required]
        public int Credits { get; set; }
        [Required]
        public decimal Price { get; set; }
        public DateTime ExpireAt { get; set; }
    }
}