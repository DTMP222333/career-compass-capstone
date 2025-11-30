using Microsoft.EntityFrameworkCore;
using Api.Models;

namespace Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<NumerologyResult> NumerologyResults { get; set; }

        public DbSet<Career> Careers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Career>().HasData(
                new Career { Id = 1, LifePath = 1, Description = "You are a natural leader — careers in management, entrepreneurship, and decision-making fit you." },
                new Career { Id = 2, LifePath = 2, Description = "You work well with people — careers in HR, counseling, teaching, or diplomacy suit you." },
                new Career { Id = 3, LifePath = 3, Description = "You are creative and expressive — fields like design, writing, or communication are ideal." },
                new Career { Id = 4, LifePath = 4, Description = "You are disciplined and practical — engineering, IT, operations, or project management fit you." },
                new Career { Id = 5, LifePath = 5, Description = "You are adaptable and energetic — sales, travel, marketing, and public-facing roles fit you well." },
                new Career { Id = 6, LifePath = 6, Description = "You are nurturing and responsible — education, healthcare, and community-focused roles suit you." },
                new Career { Id = 7, LifePath = 7, Description = "You are analytical and reflective — research, data science, IT, and technical roles are ideal." },
                new Career { Id = 8, LifePath = 8, Description = "You are ambitious and business-minded — finance, leadership, or management roles match your strengths." },
                new Career { Id = 9, LifePath = 9, Description = "You are compassionate and artistic — nonprofit, counseling, teaching, or creative work fits you well." }
            );
        }
    }
}
