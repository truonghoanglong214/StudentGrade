using Microsoft.EntityFrameworkCore;
using StudentGrade.Application.Interfaces.IRepositories;
using StudentGrade.Core.Models;
using StudentGrade.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentGrade.Infrastructure.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly StudentGradeContext _context;

        public StudentRepository(StudentGradeContext context)
        {
            _context = context;
        }

        public async Task AddRangeAsync(List<Student> students)
        {
            await _context.Students.AddRangeAsync(students);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Student>> GetByClassNameWithScoresAsync(string className)
        {
            return await _context.Students
                .Include(s => s.StudentScores)
                .ThenInclude(ss => ss.Assessment)
                .Where(s => s.ClassName == className)
                .ToListAsync();
        }

        public async Task<Student?> GetByRollNumberAsync(string rollNumber)
        {
            return await _context.Students.FirstOrDefaultAsync(s => s.RollNumber == rollNumber);
        }

        public async Task UpdateAsync(Student student)
        {
            _context.Students.Update(student);
            await _context.SaveChangesAsync();
        }
    }
}
