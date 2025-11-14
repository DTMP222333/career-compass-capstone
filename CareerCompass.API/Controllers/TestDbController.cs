using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace CareerCompass.API.Controllers
{
    [ApiController]
    [Route("api/test-db")]
    public class TestDbController : ControllerBase
    {
        private readonly IConfiguration _cfg;
        public TestDbController(IConfiguration cfg) => _cfg = cfg;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // Read the connection string from appsettings.json
            var cs = _cfg.GetConnectionString("DefaultConnection");

            try
            {
                // open connection to SQL Server
                await using var conn = new SqlConnection(cs);
                await conn.OpenAsync();

                // run a tiny query to verify the DB is alive
                await using var cmd = new SqlCommand("SELECT TOP 1 GETDATE()", conn);
                var result = await cmd.ExecuteScalarAsync();

                return Ok(new { sql = "ok", serverTime = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { sql = "error", message = ex.Message });
            }
        }
    }
}
