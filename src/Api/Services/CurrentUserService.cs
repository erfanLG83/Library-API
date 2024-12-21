using Application.Common.Interfaces;
using System.Security.Claims;

namespace Api.Services;

public class CurrentUserService : ICurrentUserService
{
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        var httpContext = httpContextAccessor.HttpContext!;
        var user = httpContext.User;

        UserId = user?.FindFirstValue(ClaimTypes.NameIdentifier);
        User = httpContext.User;
    }

    public string? UserId { get; }
    public ClaimsPrincipal? User { get; }
}