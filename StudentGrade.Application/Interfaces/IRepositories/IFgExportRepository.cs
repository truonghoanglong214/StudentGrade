using StudentGrade.Core.Models;
using System.Threading.Tasks;

namespace StudentGrade.Application.Interfaces.IRepositories
{
    public interface IFgExportRepository
    {
        Task AddAsync(Fgexport export);
    }
}
