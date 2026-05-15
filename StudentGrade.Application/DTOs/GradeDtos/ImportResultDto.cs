using System.Collections.Generic;

namespace StudentGrade.Application.DTOs.GradeDtos
{
    public class ImportResultDto
    {
        public int TotalRows { get; set; }

        public int SuccessRows { get; set; }

        public int FailedRows { get; set; }

        public bool IsSuccess => FailedRows == 0;

        public Guid ImportHistoryId { get; set; }

        public List<ImportErrorDto> Errors { get; set; } = new();
    }
}
