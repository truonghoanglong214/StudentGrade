using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentGrade.Application.DTOs.GradeDtos
{
    public class AssessmentColumnInfo
    {
        public string AssessmentCode { get; set; } = null!;

        public bool IsComment { get; set; }

        public bool IsResit { get; set; }
    }
}
