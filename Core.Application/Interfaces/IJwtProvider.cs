using Core.Domain.Entities;

namespace Core.Application.Interfaces;

public interface IJwtProvider
{
    string GenerateToken(User user);
}
