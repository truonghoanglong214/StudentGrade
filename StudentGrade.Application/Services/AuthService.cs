using AutoMapper;
using Microsoft.Extensions.Configuration;
using StudentGrade.Application.DTOs.AuthDtos;
using StudentGrade.Application.Helpers;
using StudentGrade.Application.Interfaces.IRepositories;
using StudentGrade.Application.Interfaces.IServices;
using StudentGrade.Core.Models;
using System;
using System.Threading.Tasks;

namespace StudentGrade.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthService(IUserRepository userRepository, IConfiguration configuration, IMapper mapper)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _userRepository.GetUserByUsernameAsync(request.Username);
            if (user == null)
            {
                throw new Exception("Invalid username or password");
            }

            if (!AuthHelper.VerifyPassword(request.Password, user.PasswordHash))
            {
                throw new Exception("Invalid username or password");
            }

            var token = AuthHelper.GenerateJwtToken(user, _configuration);

            return new AuthResponseDto
            {
                Token = token,
                User = _mapper.Map<UserDto>(user)
            };
        }

        public async Task<bool> RegisterAsync(RegisterRequestDto request)
        {
            if (await _userRepository.IsUsernameExists(request.Username))
            {
                throw new Exception("Tên đăng nhập đã tồn tại.");
            }

            var newUser = _mapper.Map<User>(request);
            newUser.PasswordHash = AuthHelper.HashPassword(request.Password);

            await _userRepository.AddUserAsync(newUser);

            return true;
        }

        public async Task<UserDto> GetCurrentUserAsync(Guid userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            return _mapper.Map<UserDto>(user);
        }
    }
}
