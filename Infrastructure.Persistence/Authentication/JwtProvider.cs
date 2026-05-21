using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.Application.Helper.Models;
using Core.Application.Interfaces;
using Core.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Persistence.Authentication;

public sealed class JwtProvider(IOptions<JwtSettings> jwtOptions) : IJwtProvider
{
    private readonly JwtSettings _jwtSettings = jwtOptions.Value;

    public string GenerateToken(User user)
    {
        var claims = new Claim[]
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(ClaimTypes.Role, user.Role.ToString())
        };

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _jwtSettings.Issuer,
            _jwtSettings.Audience,
            claims,
            null,
            DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            signingCredentials);

        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenValue;
    }
}
