using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentGrade.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSemesterAndMoveClassName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClassName",
                table: "Students");

            migrationBuilder.AddColumn<string>(
                name: "ClassName",
                table: "StudentScores",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Semester",
                table: "StudentScores",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClassName",
                table: "StudentScores");

            migrationBuilder.DropColumn(
                name: "Semester",
                table: "StudentScores");

            migrationBuilder.AddColumn<string>(
                name: "ClassName",
                table: "Students",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
