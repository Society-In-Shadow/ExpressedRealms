using System.Security.Claims;

namespace ExpressedRealms.Server.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetUserId(this ClaimsPrincipal principal)
    {
        return principal.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new InvalidOperationException();
    }
}