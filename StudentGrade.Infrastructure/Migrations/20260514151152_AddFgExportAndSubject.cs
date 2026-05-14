using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentGrade.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFgExportAndSubject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PMGExports");

            migrationBuilder.DropIndex(
                name: "UQ__Assessme__0C9594D7BFA84F90",
                table: "Assessments");

            migrationBuilder.AddColumn<Guid>(
                name: "SubjectId",
                table: "Assessments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "FGExports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExportedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExportedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    TotalStudents = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PMGExpor__3214EC07C5ED6A96", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FGExports_Users",
                        column: x => x.ExportedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    SubjectCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SubjectName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Subjects", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_SubjectId",
                table: "Assessments",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "UQ__Assessment_Code_Subject",
                table: "Assessments",
                columns: new[] { "AssessmentCode", "SubjectId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FGExports_ExportedAt",
                table: "FGExports",
                column: "ExportedAt");

            migrationBuilder.CreateIndex(
                name: "IX_FGExports_ExportedBy",
                table: "FGExports",
                column: "ExportedBy");

            migrationBuilder.CreateIndex(
                name: "UQ__Subjects__SubjectCode",
                table: "Subjects",
                column: "SubjectCode",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Assessments_Subjects",
                table: "Assessments",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assessments_Subjects",
                table: "Assessments");

            migrationBuilder.DropTable(
                name: "FGExports");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Assessments_SubjectId",
                table: "Assessments");

            migrationBuilder.DropIndex(
                name: "UQ__Assessment_Code_Subject",
                table: "Assessments");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "Assessments");

            migrationBuilder.CreateTable(
                name: "PMGExports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    ExportedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExportedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalStudents = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PMGExpor__3214EC07C5ED6A96", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PMGExports_Users",
                        column: x => x.ExportedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "UQ__Assessme__0C9594D7BFA84F90",
                table: "Assessments",
                column: "AssessmentCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PMGExports_ExportedAt",
                table: "PMGExports",
                column: "ExportedAt");

            migrationBuilder.CreateIndex(
                name: "IX_PMGExports_ExportedBy",
                table: "PMGExports",
                column: "ExportedBy");
        }
    }
}
