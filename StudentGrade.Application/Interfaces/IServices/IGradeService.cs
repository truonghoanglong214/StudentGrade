using StudentGrade.Application.DTOs.GradeDtos;
using System;
using System.Threading.Tasks;

namespace StudentGrade.Application.Interfaces.IServices
{
    public interface IGradeService
    {
        Task<ImportResultDto> ImportFromExcelAsync(ImportExcelRequestDto request, Guid importedBy);
        Task<ExportFgResultDto> ExportToFgAsync(ExportFgRequestDto request, Guid exportedBy);
    }
}
