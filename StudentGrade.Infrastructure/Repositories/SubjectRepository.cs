using Microsoft.EntityFrameworkCore;
using StudentGrade.Application.Interfaces.IRepositories;
using StudentGrade.Core.Models;
using StudentGrade.Infrastructure.Data;
using System.Threading.Tasks;

namespace StudentGrade.Infrastructure.Repositories
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly StudentGradeContext _context;

        public SubjectRepository(StudentGradeContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Subject subject)
        {
            await _context.Subjects.AddAsync(subject);
            await _context.SaveChangesAsync();
        }

        public async Task<Subject?> GetByCodeAsync(string code)
        {
            return await _context.Subjects.FirstOrDefaultAsync(s => s.SubjectCode == code);
        }
    }
}
