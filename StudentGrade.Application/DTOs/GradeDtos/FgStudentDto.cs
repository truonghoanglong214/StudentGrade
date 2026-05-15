using System.Collections.Generic;

namespace StudentGrade.Application.DTOs.GradeDtos
{
    public class FgStudentDto
    {
        public string Roll { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Comment { get; set; } = string.Empty;
        public List<FgGradeComponentDto> Grades { get; set; } = new();
    }
}
