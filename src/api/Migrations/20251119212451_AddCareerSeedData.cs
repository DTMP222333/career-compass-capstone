using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class AddCareerSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Careers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LifePath = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Careers", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Careers",
                columns: new[] { "Id", "Description", "LifePath" },
                values: new object[,]
                {
                    { 1, "You are a natural leader — careers in management, entrepreneurship, and decision-making fit you.", 1 },
                    { 2, "You work well with people — careers in HR, counseling, teaching, or diplomacy suit you.", 2 },
                    { 3, "You are creative and expressive — fields like design, writing, or communication are ideal.", 3 },
                    { 4, "You are disciplined and practical — engineering, IT, operations, or project management fit you.", 4 },
                    { 5, "You are adaptable and energetic — sales, travel, marketing, and public-facing roles fit you well.", 5 },
                    { 6, "You are nurturing and responsible — education, healthcare, and community-focused roles suit you.", 6 },
                    { 7, "You are analytical and reflective — research, data science, IT, and technical roles are ideal.", 7 },
                    { 8, "You are ambitious and business-minded — finance, leadership, or management roles match your strengths.", 8 },
                    { 9, "You are compassionate and artistic — nonprofit, counseling, teaching, or creative work fits you well.", 9 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Careers");
        }
    }
}
