using System.Security.Claims;
using Core.Application.Interfaces;

namespace Presentation.WebApi.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public Guid? UserId
    {
        get
        {
            var userIdString = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(userIdString, out var userId) ? userId : null;
        }
    }

    public string? Role => httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role);
}
