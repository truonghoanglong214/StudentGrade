using StudentGrade.Application.Interfaces.IRepositories;
using StudentGrade.Core.Models;
using StudentGrade.Infrastructure.Data;
using System.Threading.Tasks;

namespace StudentGrade.Infrastructure.Repositories
{
    public class ImportHistoryRepository : IImportHistoryRepository
    {
        private readonly StudentGradeContext _context;

        public ImportHistoryRepository(StudentGradeContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ImportHistory history)
        {
            await _context.ImportHistories.AddAsync(history);
            await _context.SaveChangesAsync();
        }
    }
}
