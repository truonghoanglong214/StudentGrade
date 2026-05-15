using StudentGrade.Core.Models;
using System.Threading.Tasks;

namespace StudentGrade.Application.Interfaces.IRepositories
{
    public interface IImportHistoryRepository
    {
        Task AddAsync(ImportHistory history);
    }
}
