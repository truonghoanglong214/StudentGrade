using Microsoft.EntityFrameworkCore;
using StudentGrade.Application.Interfaces.IRepositories;
using StudentGrade.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentGrade.Infrastructure.Tenancy
{
    public class Transaction : ITransaction
    {
        private readonly StudentGradeContext _context;

        public Transaction(StudentGradeContext context)
        {
            _context = context;
        }

        public async Task ExecuteAsync(Func<Task> action)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    await action(); // Chạy các hàm của Repo ở đây
                    await transaction.CommitAsync(); // Nếu ngon lành thì Commit
                }
                catch
                {
                    await transaction.RollbackAsync(); // Lỗi thì Rollback
                    throw;
                }
            });
        }
    }
}
