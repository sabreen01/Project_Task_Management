namespace Core.Application.Features.Authentication.DTOs;

public class LoginResponseDto
{
    public string AccessToken { get; set; } = string.Empty;
    public string TokenType { get; set; } = "Bearer";
}
