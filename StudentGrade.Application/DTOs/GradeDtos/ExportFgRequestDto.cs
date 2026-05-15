using System.ComponentModel.DataAnnotations;

namespace StudentGrade.Application.DTOs.GradeDtos
{
    public class ExportFgRequestDto
    {
        [Required]
        public string SubjectCode { get; set; } = null!;

        [Required]
        public string ClassName { get; set; } = null!;

        [Required]
        public string Semester { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}
