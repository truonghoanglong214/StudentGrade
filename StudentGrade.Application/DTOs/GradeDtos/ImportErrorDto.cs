using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentGrade.Application.DTOs.GradeDtos
{
    public class ImportErrorDto
    {
        public int RowNumber { get; set; }

        public string Field { get; set; } = null!;

        public string Message { get; set; } = null!;
    }
}
