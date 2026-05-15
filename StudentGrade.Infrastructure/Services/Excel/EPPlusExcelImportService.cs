using OfficeOpenXml;
using StudentGrade.Application.DTOs.GradeDtos;
using StudentGrade.Application.Interfaces.IServices;
using System.Text.RegularExpressions;

namespace StudentGrade.Infrastructure.Services.Excel
{
    public class EPPlusExcelImportService : IExcelImportService
    {
        private static readonly HashSet<string> FixedColumns =
        [
            "Class",
            "RollNumber",
            "Email",
            "MemberCode",
            "FullName",
            "ExamDate",
            "ExamNote"
        ];

        public async Task<List<ExcelStudentRowDto>> ReadAsync(Stream stream)
        {
            using var package = new ExcelPackage(stream);

            var worksheet = package.Workbook.Worksheets.FirstOrDefault();

            if (worksheet == null)
            {
                throw new Exception("Worksheet not found.");
            }

            int rowCount = worksheet.Dimension.Rows;
            int columnCount = worksheet.Dimension.Columns;

            Dictionary<int, string> headers = new();

            for (int col = 1; col <= columnCount; col++)
            {
                string header = worksheet.Cells[1, col].Text.Trim();

                if (!string.IsNullOrWhiteSpace(header))
                {
                    headers[col] = header;
                }
            }

            List<ExcelStudentRowDto> students = [];

            for (int row = 2; row <= rowCount; row++)
            {
                string rollNumber = GetCellValue(
                    worksheet,
                    row,
                    headers,
                    "RollNumber");

                if (string.IsNullOrWhiteSpace(rollNumber))
                {
                    continue;
                }

                ExcelStudentRowDto student = new()
                {
                    RowNumber = row,

                    ClassName = GetCellValue(
                        worksheet,
                        row,
                        headers,
                        "Class"),

                    RollNumber = rollNumber,

                    Email = GetCellValue(
                        worksheet,
                        row,
                        headers,
                        "Email"),

                    MemberCode = GetCellValue(
                        worksheet,
                        row,
                        headers,
                        "MemberCode"),

                    FullName = GetCellValue(
                        worksheet,
                        row,
                        headers,
                        "FullName"),

                    ExamNote = GetCellValue(
                        worksheet,
                        row,
                        headers,
                        "ExamNote")
                };

                string examDateText = GetCellValue(
                    worksheet,
                    row,
                    headers,
                    "ExamDate");

                if (DateTime.TryParse(examDateText, out DateTime examDate))
                {
                    student.ExamDate = examDate;
                }

                foreach (var header in headers)
                {
                    if (FixedColumns.Contains(header.Value))
                    {
                        continue;
                    }

                    var assessmentInfo =
                        ParseAssessmentColumn(header.Value);

                    string cellValue = worksheet
                        .Cells[row, header.Key]
                        .Text
                        .Trim();

                    var existingScore = student.Scores
                        .FirstOrDefault(x =>
                            x.AssessmentCode ==
                                assessmentInfo.AssessmentCode
                            &&
                            x.IsResit ==
                                assessmentInfo.IsResit);

                    if (existingScore == null)
                    {
                        existingScore = new ExcelAssessmentScoreDto
                        {
                            AssessmentCode =
                                assessmentInfo.AssessmentCode,

                            IsResit =
                                assessmentInfo.IsResit
                        };

                        student.Scores.Add(existingScore);
                    }

                    if (assessmentInfo.IsComment)
                    {
                        existingScore.Comment = cellValue;
                    }
                    else
                    {
                        if (decimal.TryParse(
                            cellValue,
                            out decimal score))
                        {
                            existingScore.Score = score;
                        }
                    }
                }

                students.Add(student);
            }

            return await Task.FromResult(students);
        }

        private static string GetCellValue(
            ExcelWorksheet worksheet,
            int row,
            Dictionary<int, string> headers,
            string columnName)
        {
            var entry = headers.FirstOrDefault(x => x.Value.Equals(columnName, StringComparison.OrdinalIgnoreCase));
            if (entry.Key == 0) return string.Empty; // column not found

            return worksheet.Cells[row, entry.Key].Text.Trim();
        }

        private static AssessmentColumnInfo ParseAssessmentColumn(
            string header)
        {
            header = header.Trim();

            header = Regex.Replace(header, @"\s+", " ");

            bool isComment = false;
            bool isResit = false;

            if (header.EndsWith("_Comment"))
            {
                isComment = true;

                header = header
                    .Replace("_Comment", "")
                    .Trim();
            }

            if (header.EndsWith("Resit"))
            {
                isResit = true;

                header = header
                    .Replace("Resit", "")
                    .Trim();
            }

            return new AssessmentColumnInfo
            {
                AssessmentCode = header,

                IsComment = isComment,

                IsResit = isResit
            };
        }
    }
}