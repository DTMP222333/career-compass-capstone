using Api.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Register DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Register Controllers
builder.Services.AddControllers();

// Swagger (required for .NET 8)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(_ => true);  // ⭐ ALLOW null origin
    });
});


var app = builder.Build();

// Enable Swagger ALWAYS (not only development)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Career Compass API v1");
    c.RoutePrefix = "swagger";
});



// Enable CORS
app.UseCors("AllowAll");

// Map Controllers (VERY IMPORTANT IN .NET 8)
app.MapControllers();

// Health Endpoint
app.MapGet("/api/health", () => "Healthy");

app.Run();
