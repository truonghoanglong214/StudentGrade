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
    public class AssessmentRepository : IAssessmentRepository
    {
        private readonly StudentGradeContext _context;

        public AssessmentRepository(StudentGradeContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Assessment assessment)
        {
            await _context.Assessments.AddAsync(assessment);
            await _context.SaveChangesAsync();
        }

        public async Task<Assessment?> GetByCodeAndSubjectAsync(string code, Guid subjectId)
        {
            return await _context.Assessments
                .FirstOrDefaultAsync(a => a.AssessmentCode == code && a.SubjectId == subjectId);
        }

        public async Task<List<Assessment>> GetBySubjectIdAsync(Guid subjectId)
        {
            return await _context.Assessments
                .Where(a => a.SubjectId == subjectId)
                .ToListAsync();
        }
    }
}
