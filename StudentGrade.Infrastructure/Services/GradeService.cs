using StudentGrade.Application.DTOs.GradeDtos;
using StudentGrade.Application.Interfaces.IRepositories;
using StudentGrade.Application.Interfaces.IServices;
using StudentGrade.Core.Models;

namespace StudentGrade.Infrastructure.Services
{
    public class GradeService : IGradeService
    {
        private readonly IExcelImportService _excelImportService;
        private readonly IStudentRepository _studentRepository;
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IStudentScoreRepository _studentScoreRepository;
        private readonly ITransaction _transaction;
        public GradeService(
            IExcelImportService excelImportService,
            IStudentRepository studentRepository,
            IAssessmentRepository assessmentRepository,
            IStudentScoreRepository studentScoreRepository,
            ITransaction transaction)
        {
            _excelImportService = excelImportService;
            _studentRepository = studentRepository;
            _assessmentRepository = assessmentRepository;
            _studentScoreRepository = studentScoreRepository;
            _transaction = transaction;
        }

        public async Task<ImportResultDto> ImportFromExcelAsync(
            ImportExcelRequestDto request,
            Guid importedBy)
        {

            List<StudentScore> validScores = [];
            using var stream = request.File.OpenReadStream();

            var students =
                await _excelImportService.ReadAsync(stream);
            List<ImportErrorDto> errors = [];
            foreach (var studentRow in students)
            {
                var existingStudent = await _studentRepository.IsRollNumberExists(studentRow.RollNumber);
                if (string.IsNullOrWhiteSpace(studentRow.RollNumber))
                {
                    errors.Add(new ImportErrorDto
                    {
                        RowNumber = studentRow.RowNumber,
                        Field = "RollNumber",
                        Message = "RollNumber is required."
                    });

                    continue;
                }

                if (!existingStudent)
                {
                    errors.Add(new ImportErrorDto
                    {
                        RowNumber = studentRow.RowNumber,
                        Field = "RollNumber",
                        Message = "Student not found."
                    });

                    continue;
                }
                foreach (var scoreDto in studentRow.Scores)
                {
                    var assessment = await _assessmentRepository.GetByCodeAsync(scoreDto.AssessmentCode);
                    if (assessment == null)
                    {
                        errors.Add(new ImportErrorDto
                        {
                            RowNumber = studentRow.RowNumber,
                            Field = "Assessment",
                            Message = $"Assessment '{scoreDto.AssessmentCode}' not found."
                        });
                        continue;
                    }
                    if (scoreDto.Score < 0 || scoreDto.Score > 10)
                    {
                        errors.Add(new ImportErrorDto
                        {
                            RowNumber = studentRow.RowNumber,
                            Field = scoreDto.AssessmentCode,
                            Message = "Score must be between 0 and 10."
                        });

                        continue;
                    }
                    if (scoreDto.IsResit && !assessment.IsResitSupported)
                    {
                        errors.Add(new ImportErrorDto
                        {
                            RowNumber = studentRow.RowNumber,
                            Field = scoreDto.AssessmentCode,
                            Message = "Assessment does not support resit."
                        });

                        continue;
                    }
                    var student = await _studentRepository.GetByRollNumberAsync(studentRow.RollNumber);
                    if (student == null)
                    {
                        errors.Add(new ImportErrorDto
                        {
                            RowNumber = studentRow.RowNumber,
                            Field = "RollNumber",
                            Message = "Student not found."
                        });
                        continue;
                    }
                    bool existsScore = await _studentScoreRepository.IsStudentScoreExistAsync(student.Id, assessment.Id, scoreDto.IsResit);
                    if (existsScore)
                    {
                        errors.Add(new ImportErrorDto
                        {
                            RowNumber = studentRow.RowNumber,
                            Field = scoreDto.AssessmentCode,
                            Message = "Duplicate score detected."
                        });

                        continue;
                    }
                    validScores.Add(new StudentScore
                    {
                        StudentId = student.Id,

                        AssessmentId = assessment.Id,

                        Score = scoreDto.Score,

                        IsResit = scoreDto.IsResit,

                        ExamDate = studentRow.ExamDate,

                        ExamNote = studentRow.ExamNote
                    });
                    await _transaction.ExecuteAsync(async () =>
                    {
                        await _studentScoreRepository.AddRangeAsync(validScores);
                    });

                }

            }

            return new ImportResultDto
            {
                TotalRows = students.Count,
                SuccessRows = validScores.Count,
                FailedRows = errors.Count,
                Errors = errors
            };
        }
    }
}
