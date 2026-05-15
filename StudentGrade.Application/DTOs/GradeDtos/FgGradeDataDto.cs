using System.Collections.Generic;

namespace StudentGrade.Application.DTOs.GradeDtos
{
    /// <summary>
    /// Intermediate DTO that bridges Application layer data to FuGradeLib (Infrastructure).
    /// Keeps Application layer free of any FuGradeLib dependency.
    /// </summary>
    public class FgGradeDataDto
    {
        public string Version { get; set; } = "1.0";
        public string Semester { get; set; } = null!;
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = null!;
        public List<FgSubjectClassDto> SubjectClasses { get; set; } = new();
    }
}
