using StudentGrade.Core.Models;
using System.Threading.Tasks;

namespace StudentGrade.Application.Interfaces.IRepositories
{
    public interface ISubjectRepository
    {
        Task<Subject?> GetByCodeAsync(string code);
        Task AddAsync(Subject subject);
    }
}
