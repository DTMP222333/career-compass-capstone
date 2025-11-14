namespace CareerCompass.Api.Dtos
{
    public class CareerRequestDto
    {
        public string Name { get; set; } = string.Empty;

        // We’ll assume the frontend sends this as "yyyy-MM-dd"
        public DateTime Birthdate { get; set; }
    }
}
