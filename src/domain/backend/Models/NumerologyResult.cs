namespace CareerCompass.Domain.Backend.Models
{
    public class NumerologyResult
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public int LifePathNumber { get; set; }
        public int ExpressionNumber { get; set; }
        public int SoulNumber { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
