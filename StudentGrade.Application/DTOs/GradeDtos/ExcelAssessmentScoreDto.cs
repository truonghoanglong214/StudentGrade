using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentGrade.Application.DTOs.GradeDtos
{
    public class ExcelAssessmentScoreDto
    {
        public string AssessmentCode { get; set; } = null!;

        public decimal? Score { get; set; }

        public string? Comment { get; set; }

        public bool IsResit { get; set; }
    }
}
