using Microsoft.EntityFrameworkCore;
using StudentGrade.Application.Interfaces.IRepositories;
using StudentGrade.Core.Models;
using StudentGrade.Infrastructure.Data;

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

        public async Task AddAsync(Student student)
        {
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Student>> GetByClassAndSemesterWithScoresAsync(string className, string semester)
        {
            return await _context.Students
                .Include(s => s.StudentScores.Where(ss => ss.ClassName == className && ss.Semester == semester))
                .ThenInclude(ss => ss.Assessment)
                .Where(s => s.StudentScores.Any(ss => ss.ClassName == className && ss.Semester == semester))
                .ToListAsync();
        }

        public async Task<Student?> GetByIdAsync(Guid studentId)
        {
            return await _context.Students.FirstOrDefaultAsync(x => x.Id == studentId);
        }

        public async Task<Student?> GetByRollNumberAsync(string rollNumber)
        {
            return await _context.Students.FirstOrDefaultAsync(s => s.RollNumber == rollNumber);
        }

        public async Task<bool> IsRollNumberExists(string rollNumber)
        {
            return await _context.Students.AnyAsync(x => x.RollNumber == rollNumber);
        }

        public async Task UpdateAsync(Student student)
        {
            _context.Students.Update(student);
            await _context.SaveChangesAsync();
        }
    }
}
