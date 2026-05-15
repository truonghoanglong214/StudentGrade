using StudentGrade.Application.DTOs.GradeDtos;
using StudentGrade.Application.Interfaces.IRepositories;
using StudentGrade.Application.Interfaces.IServices;
using StudentGrade.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentGrade.Infrastructure.Services
{
    public class GradeService : IGradeService
    {
        private readonly IExcelImportService _excelImportService;
        private readonly IStudentRepository _studentRepository;
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IStudentScoreRepository _studentScoreRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly IImportHistoryRepository _importHistoryRepository;
        private readonly IFgExportRepository _fgExportRepository;
        private readonly IFGSerializer _fgSerializer;
        private readonly ITransaction _transaction;

        public GradeService(
            IExcelImportService excelImportService,
            IStudentRepository studentRepository,
            IAssessmentRepository assessmentRepository,
            IStudentScoreRepository studentScoreRepository,
            ISubjectRepository subjectRepository,
            IImportHistoryRepository importHistoryRepository,
            IFgExportRepository fgExportRepository,
            IFGSerializer fgSerializer,
            ITransaction transaction)
        {
            _excelImportService = excelImportService;
            _studentRepository = studentRepository;
            _assessmentRepository = assessmentRepository;
            _studentScoreRepository = studentScoreRepository;
            _subjectRepository = subjectRepository;
            _importHistoryRepository = importHistoryRepository;
            _fgExportRepository = fgExportRepository;
            _fgSerializer = fgSerializer;
            _transaction = transaction;
        }

        public async Task<ImportResultDto> ImportFromExcelAsync(
            ImportExcelRequestDto request,
            Guid importedBy)
        {
            using var stream = request.File.OpenReadStream();
            var studentRows = await _excelImportService.ReadAsync(stream);

            var subject = await _subjectRepository.GetByCodeAsync(request.SubjectCode);
            if (subject == null)
            {
                subject = new Subject
                {
                    SubjectCode = request.SubjectCode,
                    SubjectName = request.SubjectCode   
                };
                await _subjectRepository.AddAsync(subject);
            }

            List<StudentScore> validScores = new();
            List<ImportErrorDto> errors = new();

            foreach (var studentRow in studentRows)
            {
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

                var student = await _studentRepository.GetByRollNumberAsync(studentRow.RollNumber);
                if (student == null)
                {
                    student = new Student
                    {
                        RollNumber = studentRow.RollNumber,
                        MemberCode = studentRow.MemberCode,
                        FullName = studentRow.FullName,
                        Email = studentRow.Email,
                        ClassName = studentRow.ClassName,
                        CreatedAt = DateTime.UtcNow
                    };
                    await _studentRepository.AddAsync(student);
                }

                foreach (var scoreDto in studentRow.Scores)
                {
                    var assessment = await _assessmentRepository
                        .GetByCodeAndSubjectAsync(scoreDto.AssessmentCode, subject.Id);

                    if (assessment == null)
                    {
                        assessment = new Assessment
                        {
                            SubjectId = subject.Id,
                            AssessmentCode = scoreDto.AssessmentCode,
                            AssessmentName = scoreDto.AssessmentCode,
                            Weight = 0,
                            IsResitSupported = true
                        };
                        await _assessmentRepository.AddAsync(assessment);
                    }

                    if (scoreDto.Score.HasValue && (scoreDto.Score < 0 || scoreDto.Score > 10))
                    {
                        errors.Add(new ImportErrorDto
                        {
                            RowNumber = studentRow.RowNumber,
                            Field = scoreDto.AssessmentCode,
                            Message = $"Score {scoreDto.Score} is out of range (must be 0–10)."
                        });
                        continue;
                    }


                    if (scoreDto.IsResit && !assessment.IsResitSupported)
                    {
                        errors.Add(new ImportErrorDto
                        {
                            RowNumber = studentRow.RowNumber,
                            Field = scoreDto.AssessmentCode,
                            Message = $"Assessment '{scoreDto.AssessmentCode}' does not support resit."
                        });
                        continue;
                    }

                    bool alreadyExists = await _studentScoreRepository
                        .IsStudentScoreExistAsync(student.Id, assessment.Id, scoreDto.IsResit);

                    if (alreadyExists)
                    {
                        errors.Add(new ImportErrorDto
                        {
                            RowNumber = studentRow.RowNumber,
                            Field = scoreDto.AssessmentCode,
                            Message = $"Score for '{scoreDto.AssessmentCode}' (IsResit={scoreDto.IsResit}) already exists."
                        });
                        continue;
                    }

                    validScores.Add(new StudentScore
                    {
                        StudentId   = student.Id,
                        AssessmentId = assessment.Id,
                        Score       = scoreDto.Score,
                        Comment     = scoreDto.Comment,
                        IsResit     = scoreDto.IsResit,
                        ExamDate    = studentRow.ExamDate,
                        ExamNote    = studentRow.ExamNote
                    });
                }
            }

            var importHistory = new ImportHistory
            {
                FileName    = request.File.FileName,
                FilePath    = string.Empty,
                ImportedBy  = importedBy,
                TotalRows   = studentRows.Count,
                SuccessRows = validScores.Count,
                FailedRows  = errors.Count,
                Status      = errors.Count == 0 ? "Success" : "Partial"
            };

            await _transaction.ExecuteAsync(async () =>
            {
                await _importHistoryRepository.AddAsync(importHistory);
                foreach (var score in validScores)
                    score.ImportedFileId = importHistory.Id;

                if (validScores.Count > 0)
                    await _studentScoreRepository.AddRangeAsync(validScores);
            });

            return new ImportResultDto
            {
                TotalRows       = studentRows.Count,
                SuccessRows     = validScores.Count,
                FailedRows      = errors.Count,
                ImportHistoryId = importHistory.Id,
                Errors          = errors
            };
        }

        
        public async Task<ExportFgResultDto> ExportToFgAsync(
            ExportFgRequestDto request,
            Guid exportedBy)
        {
            var subject = await _subjectRepository.GetByCodeAsync(request.SubjectCode)
                ?? throw new InvalidOperationException($"Subject '{request.SubjectCode}' not found.");

            var assessments = await _assessmentRepository.GetBySubjectIdAsync(subject.Id);
            if (assessments.Count == 0)
                throw new InvalidOperationException($"No assessments found for subject '{request.SubjectCode}'.");

            var students = await _studentRepository.GetByClassNameWithScoresAsync(request.ClassName);
            if (students.Count == 0)
                throw new InvalidOperationException($"No students found in class '{request.ClassName}'.");

            
            var components = new List<string>();
            foreach (var a in assessments)
            {
                components.Add(a.AssessmentCode);
                if (a.IsResitSupported)
                    components.Add($"{a.AssessmentCode} Resit");
            }

            //  Map each Student → FgStudentDto
            var fgStudents = students.Select(s =>
            {
                var grades = new List<FgGradeComponentDto>();

                foreach (var assessment in assessments)
                {
                    // Regular score
                    var normalScore = s.StudentScores
                        .FirstOrDefault(ss => ss.AssessmentId == assessment.Id && !ss.IsResit);
                    grades.Add(new FgGradeComponentDto
                    {
                        Component = assessment.AssessmentCode,
                        Grade = normalScore?.Score.HasValue == true
                            ? (float?)decimal.ToSingle(normalScore.Score!.Value)
                            : null
                    });

                    // Resit score (separate component)
                    if (assessment.IsResitSupported)
                    {
                        var resitScore = s.StudentScores
                            .FirstOrDefault(ss => ss.AssessmentId == assessment.Id && ss.IsResit);
                        grades.Add(new FgGradeComponentDto
                        {
                            Component = $"{assessment.AssessmentCode} Resit",
                            Grade = resitScore?.Score.HasValue == true
                                ? (float?)decimal.ToSingle(resitScore.Score!.Value)
                                : null
                        });
                    }
                }

                return new FgStudentDto
                {
                    Roll    = s.RollNumber,
                    Name    = s.FullName,
                    Comment = s.StudentScores.FirstOrDefault()?.Comment ?? string.Empty,
                    Grades  = grades
                };
            }).ToList();

            // Assemble the full FgGradeDataDto
            var fgData = new FgGradeDataDto
            {
                Version  = "1.0",
                Semester = request.Semester,
                Login    = string.Empty,
                Password = request.Password,
                SubjectClasses = new List<FgSubjectClassDto>
                {
                    new FgSubjectClassDto
                    {
                        Subject    = request.SubjectCode,
                        Class      = request.ClassName,
                        Components = components,
                        Students   = fgStudents
                    }
                }
            };

            // Serialize to binary .fg
            var fileBytes = _fgSerializer.Serialize(fgData);
            var fileName  = $"{request.SubjectCode}_{request.ClassName}_{request.Semester}.fg";

            // Log the export
            var exportRecord = new Fgexport
            {
                FileName       = fileName,
                FilePath       = string.Empty,
                ExportedBy     = exportedBy,
                TotalStudents  = students.Count
            };
            await _fgExportRepository.AddAsync(exportRecord);

            return new ExportFgResultDto
            {
                FileName    = fileName,
                FileContent = fileBytes
            };
        }
    }
}
