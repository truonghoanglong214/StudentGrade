using StudentGrade.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentGrade.Application.Interfaces.IRepositories
{
    public interface IStudentScoreRepository
    {
        Task<List<StudentScore>> GetByClassNameAsync(string className);
        Task AddRangeAsync(List<StudentScore> scores);
    }
}
