using Core.Application.Helper.Models;
using Core.Application.Interfaces;
using Core.Domain.Entities;
using Core.Domain.Enums;
using MediatR;


namespace Core.Application.Features.Authentication.Commands;

public record RegisterCommand(string FirstName, string LastName, string Email, string Password) 
    : IRequest<RequestResult<Guid>>;

public class RegisterCommandHandler(
    IRepository<User> userRepository,
    IPasswordHasher passwordHasher) 
    : IRequestHandler<RegisterCommand, RequestResult<Guid>>
{
    public async Task<RequestResult<Guid>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await userRepository.GetFirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
        
        if (existingUser != null)
            return RequestResult<Guid>.Failure("Email is already registered.");

        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PasswordHash = passwordHasher.Hash(request.Password),
            Role = Role.User
        };

        await userRepository.AddAsync(user, cancellationToken);
        await userRepository.SaveChangesAsync(cancellationToken);

        return RequestResult<Guid>.Success(user.Id, "User registered successfully.");
    }
}
