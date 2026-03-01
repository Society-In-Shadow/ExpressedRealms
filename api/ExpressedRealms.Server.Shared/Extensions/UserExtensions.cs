using System.Security.Claims;
using ExpressedRealms.Authentication.PermissionCollection.Support;

namespace ExpressedRealms.Server.Shared.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetUserId(this ClaimsPrincipal principal)
    {
        return principal.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new InvalidOperationException();
    }
    
    public static bool UserHasPermission(this ClaimsPrincipal principal, Permission permission)
    {
        return principal.HasClaim("custom_permission", permission.Key);
    }
}
