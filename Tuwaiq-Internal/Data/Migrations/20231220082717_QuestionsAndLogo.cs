using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TuwaiqRecruitment.Data.Migrations
{
    /// <inheritdoc />
    public partial class QuestionsAndLogo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Logo",
                table: "Company",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Questions",
                table: "Candidate",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Logo",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "Questions",
                table: "Candidate");
        }
    }
}
