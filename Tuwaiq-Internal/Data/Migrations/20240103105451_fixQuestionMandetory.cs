using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TuwaiqRecruitment.Data.Migrations
{
    /// <inheritdoc />
    public partial class fixQuestionMandetory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Questions",
                table: "Candidate",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Candidate",
                keyColumn: "Questions",
                keyValue: null,
                column: "Questions",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Questions",
                table: "Candidate",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
