using Microsoft.EntityFrameworkCore;
using StudentGrade.Application.Interfaces.IRepositories;
using StudentGrade.Core.Models;
using StudentGrade.Infrastructure.Data;

namespace StudentGrade.Infrastructure.Repositories
{
    public class StudentScoreRepository : IStudentScoreRepository
    {
        private readonly StudentGradeContext _context;

        public StudentScoreRepository(StudentGradeContext context)
        {
            _context = context;
        }

        public async Task AddRangeAsync(List<StudentScore> scores)
        {
            await _context.StudentScores.AddRangeAsync(scores);
            await _context.SaveChangesAsync();
        }

        public async Task<List<StudentScore>> GetByClassNameAsync(string className)
        {
            return await _context.StudentScores
                .Include(s => s.Assessment)
                .Where(s => s.Student.ClassName == className)
                .ToListAsync();
        }

        public async Task<bool> IsStudentScoreExistAsync(Guid studentId, Guid assessmentId, bool isResit)
        {
            return await _context.StudentScores.AnyAsync(x => x.Id == studentId && x.AssessmentId == assessmentId && x.IsResit == isResit);
        }
    }
}
