using System.Security.Claims;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace ExpressedRealms.Admin.API.CustomClaimsPrincipleFactory;

public class RedisClaimsTransformer(ClaimStash claimStash, UserManager<User> userManager) : IClaimsTransformation
{
    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        if (!principal.Identity?.IsAuthenticated ?? true)
            return principal;
        
        var nameIdentifier = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (nameIdentifier == null)
            return principal;

        var claims = await claimStash.GetClaimsFromCache(principal, nameIdentifier);

        var identity = principal.Identity as ClaimsIdentity;
        foreach (var c in claims)
        {
            if (!identity.HasClaim(c.Type, c.Value))
            {
                identity.AddClaim(new Claim(c.Type, c.Value));
            }
        }

        return principal;
    }
}
