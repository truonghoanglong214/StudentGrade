using StudentGrade.Application.DTOs.AuthDtos;
using System;
using System.Threading.Tasks;

namespace StudentGrade.Application.Interfaces.IServices
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginRequestDto request);
        Task<bool> RegisterAsync(RegisterRequestDto request);
        Task<UserDto> GetCurrentUserAsync(Guid userId);
    }
}
