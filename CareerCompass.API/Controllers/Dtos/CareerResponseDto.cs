namespace CareerCompass.Api.Dtos
{
    public class CareerResponseDto
    {
        public string Career { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public NumerologyNumbersDto Numbers { get; set; } = new NumerologyNumbersDto();
    }

    public class NumerologyNumbersDto
    {
        public int LifePath { get; set; }
        public int Expression { get; set; }
        public int Personality { get; set; }
        public int SoulUrge { get; set; }
        public int Destiny { get; set; }
    }
}
