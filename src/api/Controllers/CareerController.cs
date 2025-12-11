using Microsoft.AspNetCore.Mvc;
using Api.Data;
using Api.Models;
using Api.Services;
using System.Linq;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CareerController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public CareerController(ApplicationDbContext db)
        {
            _db = db;
        }

        // POST: /api/career/recommendation
        [HttpPost("recommendation")]
        public IActionResult Recommend([FromBody] CareerRequest request)
        {
            if (request == null ||
                string.IsNullOrWhiteSpace(request.FullName) ||
                string.IsNullOrWhiteSpace(request.DateOfBirth))
            {
                return BadRequest(new { message = "Invalid request." });
            }

            // Convert DOB
            if (!DateTime.TryParse(request.DateOfBirth, out DateTime dob))
                return BadRequest(new { message = "Invalid date format. Use YYYY-MM-DD." });

            // Calculate Life Path
            int lifePath = NumerologyCalculator.GetLifePath(dob);

            // Fetch full career meaning from database (Careers table)
            var career = _db.Careers.FirstOrDefault(x => x.LifePath == lifePath);

            if (career == null)
            {
                return NotFound(new
                {
                    message = "No career match found.",
                    lifePath = lifePath
                });
            }

            // Build clean response
            return Ok(new
            {
                fullName = request.FullName,
                lifePath = lifePath,

                // Short title
                career = NumerologyCalculator.GetCareerTitle(lifePath),

                // Full meaning from DB
                description = career.Description
            });
        }

        // GET: /api/career
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_db.Careers.ToList());
        }

        // GET: /api/career/{lifePath}
        [HttpGet("{lifePath}")]
        public IActionResult GetCareer(int lifePath)
        {
            var career = _db.Careers.FirstOrDefault(x => x.LifePath == lifePath);

            if (career == null)
                return NotFound("Career description not found for this Life Path.");

            return Ok(career);
        }
    }
}

