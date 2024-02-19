using System.Security.Claims;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Identity;

namespace ExpressedRealms.Server.EndPoints;

internal static class AuthEndPoints
{
    internal static void AddAuthEndPoints(this WebApplication app)
    {
        app.MapGroup("auth").MapIdentityApi<IdentityUser>();
        app.MapGroup("auth").MapPost("/logoff", (HttpContext httpContext) => Results.SignOut());
        app.MapGroup("auth").MapGet("/getAntiforgeryToken", (IAntiforgery _antiforgery, HttpContext httpContext, ClaimsPrincipal user) =>
        {
            var tokens = _antiforgery.GetAndStoreTokens(httpContext);
            httpContext.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken,
                new CookieOptions() { HttpOnly = false });
            return Results.Ok();
        });
    }
}