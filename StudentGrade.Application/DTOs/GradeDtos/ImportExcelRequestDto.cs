using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace StudentGrade.Application.DTOs.GradeDtos
{
    public class ImportExcelRequestDto
    {
        [Required]
        public IFormFile File { get; set; } = null!;

        [Required]
        public Guid SubjectId { get; set; }

        [Required]
        public Guid AssessmentId { get; set; }  
    }
}
