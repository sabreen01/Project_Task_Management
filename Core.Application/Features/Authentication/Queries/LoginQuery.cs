using Core.Application.Features.Authentication.DTOs;
using Core.Application.Helper.Models;
using Core.Application.Interfaces;
using Core.Domain.Entities;
using MediatR;

namespace Core.Application.Features.Authentication.Queries;

public record LoginQuery(string Email, string Password) : IRequest<RequestResult<LoginResponseDto>>;

public class LoginQueryHandler(
    IRepository<User> userRepository,
    IPasswordHasher passwordHasher,
    IJwtProvider jwtProvider) 
    : IRequestHandler<LoginQuery, RequestResult<LoginResponseDto>>
{
    public async Task<RequestResult<LoginResponseDto>> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetFirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        if (user == null)
            return RequestResult<LoginResponseDto>.Failure("Invalid email or password.");

        bool isValidPassword = passwordHasher.Verify(request.Password, user.PasswordHash);
        if (!isValidPassword)
            return RequestResult<LoginResponseDto>.Failure("Invalid email or password.");

        var token = jwtProvider.GenerateToken(user);

        var response = new LoginResponseDto
        {
            AccessToken = token
        };

        return RequestResult<LoginResponseDto>.Success(response, "Login successful.");
    }
}
