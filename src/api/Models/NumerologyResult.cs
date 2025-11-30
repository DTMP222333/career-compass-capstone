public class NumerologyResult
{
    public int Id { get; set; }

    public string? FullName { get; set; }
    public DateTime DateOfBirth { get; set; }

    public int LifePath { get; set; }
    public int Expression { get; set; }
    public int SoulUrge { get; set; }
    public int Personality { get; set; }
    public int BirthDay { get; set; }

    public string? Career { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
