namespace Api.Models
{
    public class Career
    {
        public int Id { get; set; }

        // Life Path number this career belongs to
        public int LifePath { get; set; }

        // Description of the career (Required)
        public string Description { get; set; } = string.Empty;
    }
}
