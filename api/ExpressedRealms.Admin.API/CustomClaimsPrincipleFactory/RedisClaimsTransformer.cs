using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ExpressedRealms.Admin.API.CustomClaimsPrincipleFactory;

public class RedisClaimsTransformer(
    ClaimStash claimStash,
    IHttpContextAccessor httpContext,
    IWebHostEnvironment env,
    ILogger<RedisClaimsTransformer> logger
) : IClaimsTransformation
{
    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var items = httpContext.HttpContext!.Items;
        var nameIdentifier = principal
            .Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
            ?.Value;

        if (nameIdentifier == null)
        {
            if (env.IsDevelopment())
            {
                throw new UnauthorizedAccessException(
                    "No NameIdentifier claim found in principal.  Probably unauthenticated.  Kicking User Out..."
                );
            }

            logger.LogWarning("No NameIdentifier claim found in principal.  Kicking User Out.");
            return new ClaimsPrincipal(new ClaimsIdentity());
        }

        if (items[$"ClaimsTransformed:{nameIdentifier}"] is ClaimsPrincipal cached)
            return cached;

        var claims = await claimStash.GetClaimsFromCache(principal, nameIdentifier);

        var identity = principal.Identity as ClaimsIdentity;
        foreach (var c in claims.Where(c => !identity!.HasClaim(c.Type, c.Value)))
        {
            identity!.AddClaim(new Claim(c.Type, c.Value));
        }

        // Middleware can cause this to be run multiple times, so cache the transformed claims
        items[$"ClaimsTransformed:{nameIdentifier}"] = principal;
        return principal;
    }
}
