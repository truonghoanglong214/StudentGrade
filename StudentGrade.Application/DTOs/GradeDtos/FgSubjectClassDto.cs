using System.Collections.Generic;

namespace StudentGrade.Application.DTOs.GradeDtos
{
    public class FgSubjectClassDto
    {
        public string Subject { get; set; } = null!;
        public string Class { get; set; } = null!;
        public List<string> Components { get; set; } = new();
        public List<FgStudentDto> Students { get; set; } = new();
    }
}
