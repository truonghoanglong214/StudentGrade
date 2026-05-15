using StudentGrade.Application.Interfaces.IRepositories;
using StudentGrade.Core.Models;
using StudentGrade.Infrastructure.Data;
using System.Threading.Tasks;

namespace StudentGrade.Infrastructure.Repositories
{
    public class FgExportRepository : IFgExportRepository
    {
        private readonly StudentGradeContext _context;

        public FgExportRepository(StudentGradeContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Fgexport export)
        {
            await _context.Fgexports.AddAsync(export);
            await _context.SaveChangesAsync();
        }
    }
}
