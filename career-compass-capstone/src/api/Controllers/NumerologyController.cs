using Api.Data;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NumerologyController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public NumerologyController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var results = await _db.NumerologyResults
                .OrderByDescending(x => x.Id)
                .Take(10)
                .ToListAsync();

            return Ok(results);
        }
        [HttpPost("calculate")]
        public async Task<IActionResult> Calculate([FromBody] NumerologyRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Name validation
            request.FullName = request.FullName?.Trim();

            if (string.IsNullOrWhiteSpace(request.FullName) || request.FullName.Length < 2)
                return BadRequest("Please enter a valid full name.");

            if (!System.Text.RegularExpressions.Regex.IsMatch(request.FullName, @"^[A-Za-z]+(?: [A-Za-z]+)*$"))
                return BadRequest("Full name can only contain letters and spaces. No numbers or symbols allowed.");

            var lifePath = NumerologyCalculator.GetLifePath(request.DateOfBirth);
            var expression = NumerologyCalculator.GetExpression(request.FullName);
            var soulUrge = NumerologyCalculator.GetSoulUrge(request.FullName);
            var personality = NumerologyCalculator.GetPersonality(request.FullName);
            var birthday = NumerologyCalculator.GetBirthDay(request.DateOfBirth);

            var careerMeaning = await _db.Careers
                .Where(c => c.LifePath == lifePath)
                .Select(c => c.Description)
                .FirstOrDefaultAsync();

            if (careerMeaning == null)
                careerMeaning = "No career description found.";

            var careerTitle = NumerologyCalculator.GetCareerTitle(lifePath);

            var result = new NumerologyResult
            {
                FullName = request.FullName,
                DateOfBirth = request.DateOfBirth,
                LifePath = lifePath,
                Expression = expression,
                SoulUrge = soulUrge,
                Personality = personality,
                BirthDay = birthday,
                Career = careerTitle,
                CreatedAt = DateTime.UtcNow
            };

            _db.NumerologyResults.Add(result);
            await _db.SaveChangesAsync();

            return Ok(new
            {
                id = result.Id,
                fullName = result.FullName,
                dateOfBirth = result.DateOfBirth,
                lifePath = result.LifePath,
                expression = result.Expression,
                soulUrge = result.SoulUrge,
                personality = result.Personality,
                birthDay = result.BirthDay,
                career = careerTitle,
                careerMeaning = careerMeaning,
                createdAt = result.CreatedAt
            });
        }
    }
}

