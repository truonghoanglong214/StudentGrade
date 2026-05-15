using Microsoft.EntityFrameworkCore;
using StudentGrade.Application.Interfaces.IRepositories;
using StudentGrade.Core.Models;
using StudentGrade.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                .Include(s => s.Student)
                .Include(s => s.Assessment)
                .Where(s => s.ClassName == className)
                .ToListAsync();
        }

        public async Task<bool> IsStudentScoreExistAsync(Guid studentId, Guid assessmentId, string semester, bool isResit)
        {
            // Bug fix: was checking x.Id == studentId, should be x.StudentId == studentId
            return await _context.StudentScores
                .AnyAsync(x => x.StudentId == studentId
                            && x.AssessmentId == assessmentId
                            && x.Semester == semester
                            && x.IsResit == isResit);
        }
    }
}
