using StudentGrade.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentGrade.Application.Interfaces.IRepositories
{
    public interface IUserRepository
    {
        Task AddUserAsync(User user);
        Task<bool> IsUsernameExists(string userName);
        Task<User?> GetUserByIdAsync(Guid id);

    }
}
