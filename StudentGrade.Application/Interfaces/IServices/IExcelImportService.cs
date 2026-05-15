using StudentGrade.Application.DTOs.GradeDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentGrade.Application.Interfaces.IServices
{
    public interface IExcelImportService
    {
        Task<List<ExcelStudentRowDto>> ReadAsync(Stream stream);
    }
}
