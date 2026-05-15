using StudentGrade.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentGrade.Application.Interfaces.IRepositories
{
    public interface IStudentRepository
    {
        Task<Student?> GetByRollNumberAsync(string rollNumber);
        Task<List<Student>> GetByClassAndSemesterWithScoresAsync(string className, string semester);
        Task AddAsync(Student student);
        Task AddRangeAsync(List<Student> students);
        Task UpdateAsync(Student student);
        Task<bool> IsRollNumberExists(string rollNumber);
    }
}
