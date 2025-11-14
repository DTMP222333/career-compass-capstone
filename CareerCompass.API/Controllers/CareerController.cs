using CareerCompass.Api.Dtos;
using CareerCompass.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CareerCompass.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CareerController : ControllerBase
    {
        private readonly INumerologyService _numerologyService;

        public CareerController(INumerologyService numerologyService)
        {
            _numerologyService = numerologyService;
        }

        // POST: /api/career/recommendation
        [HttpPost("recommendation")]
        public ActionResult<CareerResponseDto> GetRecommendation([FromBody] CareerRequestDto request)
        {
            if (request == null)
            {
                return BadRequest("Request body is required.");
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest("Name is required.");
            }

            if (request.Birthdate == default)
            {
                return BadRequest("Birthdate is required.");
            }

            var result = _numerologyService.GetCareerRecommendation(request.Name, request.Birthdate);
            return Ok(result);
        }
    }
}
