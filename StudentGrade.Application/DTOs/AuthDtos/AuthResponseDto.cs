namespace StudentGrade.Application.DTOs.AuthDtos
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = null!;
        public UserDto User { get; set; } = null!;
    }
}
