using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentGrade.API.Helpers;
using StudentGrade.Application.DTOs.GradeDtos;
using StudentGrade.Application.Interfaces.IServices;
using System;
using System.Threading.Tasks;

namespace StudentGrade.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GradeController : ControllerBase
    {
        private readonly IGradeService _gradeService;
        private readonly ICurrentUserHelper _currentUserHelper;

        public GradeController(IGradeService gradeService, ICurrentUserHelper currentUserHelper)
        {
            _gradeService = gradeService;
            _currentUserHelper = currentUserHelper;
        }

        /// <summary>
        /// Import danh sách điểm sinh viên từ file Excel (.xlsx).
        /// </summary>
        [HttpPost("import")]
        public async Task<IActionResult> ImportExcel([FromForm] ImportExcelRequestDto request)
        {
            var userId = _currentUserHelper.GetUserId();
            if (userId == null)
                return Unauthorized();

            if (request.File == null || request.File.Length == 0)
                return BadRequest("File is required.");

            var result = await _gradeService.ImportFromExcelAsync(request, userId.Value);
            return Ok(result);
        }

        /// <summary>
        /// Xuất điểm ra file .fg (FUGE format) cho một môn học và lớp cụ thể.
        /// </summary>
        [HttpPost("export-fg")]
        public async Task<IActionResult> ExportFg([FromBody] ExportFgRequestDto request)
        {
            var userId = _currentUserHelper.GetUserId();
            if (userId == null)
                return Unauthorized();

            var result = await _gradeService.ExportToFgAsync(request, userId.Value);

            return File(result.FileContent, result.ContentType, result.FileName);
        }
    }
}
