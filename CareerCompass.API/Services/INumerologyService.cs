using CareerCompass.Api.Dtos;

namespace CareerCompass.Api.Services
{
    public interface INumerologyService
    {
        CareerResponseDto GetCareerRecommendation(string fullName, DateTime birthDate);
    }
}
