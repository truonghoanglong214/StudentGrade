using StudentGrade.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;

namespace StudentGrade.Application.Interfaces.IRepositories
{
    public interface IStudentRepository
    {
        Task<Student?> GetByRollNumberAsync(string rollNumber);
        Task<List<Student>> GetByClassNameWithScoresAsync(string className);
        Task AddRangeAsync(List<Student> students);
        Task UpdateAsync(Student student);
        Task<bool?> IsRollNumberExists(string rollNumber);
    }
}
