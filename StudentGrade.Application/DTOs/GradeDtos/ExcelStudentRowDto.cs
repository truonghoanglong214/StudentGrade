using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentGrade.Application.DTOs.GradeDtos
{
    public class ExcelStudentRowDto
    {
        public int RowNumber { get; set; }

        public string ClassName { get; set; } = null!;

        public string RollNumber { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string MemberCode { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public DateTime? ExamDate { get; set; }

        public string? ExamNote { get; set; }

        public List<ExcelAssessmentScoreDto> Scores { get; set; } = [];
    }
}
