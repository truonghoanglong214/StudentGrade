using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentGrade.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Assessments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    AssessmentCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AssessmentName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Weight = table.Column<double>(type: "float", nullable: false),
                    IsResitSupported = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Assessme__3214EC076B4659BD", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    RollNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MemberCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ClassName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Students__3214EC077F4D1606", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Users__3214EC0773D05D9B", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AIChatHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AskedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__AIChatHi__3214EC07FEEC9772", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AIChatHistories_Users",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__AuditLog__3214EC07EFC5399F", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditLogs_Users",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ImportHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImportedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ImportedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    TotalRows = table.Column<int>(type: "int", nullable: false),
                    SuccessRows = table.Column<int>(type: "int", nullable: false),
                    FailedRows = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ImportHi__3214EC077F18FA3B", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImportHistories_Users",
                        column: x => x.ImportedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PMGExports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExportedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExportedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
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

            migrationBuilder.CreateTable(
                name: "StudentScores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssessmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Score = table.Column<double>(type: "float", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsResit = table.Column<bool>(type: "bit", nullable: false),
                    ExamDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExamNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImportedFileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__StudentS__3214EC07BA001336", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentScores_Assessments",
                        column: x => x.AssessmentId,
                        principalTable: "Assessments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StudentScores_ImportHistories",
                        column: x => x.ImportedFileId,
                        principalTable: "ImportHistories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StudentScores_Students",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AIChatHistories_UserId",
                table: "AIChatHistories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "UQ__Assessme__0C9594D7BFA84F90",
                table: "Assessments",
                column: "AssessmentCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_UserId",
                table: "AuditLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportHistories_ImportedAt",
                table: "ImportHistories",
                column: "ImportedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ImportHistories_ImportedBy",
                table: "ImportHistories",
                column: "ImportedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PMGExports_ExportedAt",
                table: "PMGExports",
                column: "ExportedAt");

            migrationBuilder.CreateIndex(
                name: "IX_PMGExports_ExportedBy",
                table: "PMGExports",
                column: "ExportedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Students_RollNumber",
                table: "Students",
                column: "RollNumber");

            migrationBuilder.CreateIndex(
                name: "UQ__Students__E9F06F16CA514B74",
                table: "Students",
                column: "RollNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentScores_AssessmentId",
                table: "StudentScores",
                column: "AssessmentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentScores_ImportedFileId",
                table: "StudentScores",
                column: "ImportedFileId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentScores_StudentId",
                table: "StudentScores",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "UQ__Users__536C85E42BC6DF6C",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AIChatHistories");

            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "PMGExports");

            migrationBuilder.DropTable(
                name: "StudentScores");

            migrationBuilder.DropTable(
                name: "Assessments");

            migrationBuilder.DropTable(
                name: "ImportHistories");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
