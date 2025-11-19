using Api.Data;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> Calculate([FromBody] NumerologyRequest req)
        {
            int lifePath = NumerologyCalculator.GetLifePath(req.DateOfBirth);
            int expression = NumerologyCalculator.GetExpression(req.FullName);
            int soulUrge = NumerologyCalculator.GetSoulUrge(req.FullName);
            int personality = NumerologyCalculator.GetPersonality(req.FullName);
            int birthDay = NumerologyCalculator.GetBirthDay(req.DateOfBirth);

            var result = new NumerologyResult
            {
                FullName = req.FullName,
                DateOfBirth = req.DateOfBirth,
                LifePath = lifePath,
                Expression = expression,
                SoulUrge = soulUrge,
                Personality = personality,
                BirthDay = birthDay
            };

            _db.NumerologyResults.Add(result);
            await _db.SaveChangesAsync();

            return Ok(result);
        }
    }
}
