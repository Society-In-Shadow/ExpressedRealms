using System.Security.Claims;
using ExpressedRealms.DB.UserProfile.PlayerDBModels;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http.HttpResults;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace ExpressedRealms.Server.EndPoints;

internal static class AuthEndPoints
{
    internal static void AddAuthEndPoints(this WebApplication app)
    {
        var endpointGroup = app.MapGroup("auth")
            .AddFluentValidationAutoValidation()
            .WithTags("Authentication")
            .WithOpenApi();
        
        endpointGroup.MapIdentityApi<User>();
        endpointGroup.MapPost("/logoff", (HttpContext httpContext) => Results.SignOut());
        endpointGroup.MapGet("/getAntiforgeryToken", Results<NoContent, StatusCodeHttpResult>(IAntiforgery antiforgery, HttpContext httpContext, ClaimsPrincipal user) =>
        {
            var tokens = antiforgery.GetAndStoreTokens(httpContext);

            if (tokens.RequestToken is null)
            {
                app.Logger.LogCritical("The anti-forgery token was not generated.");
                return TypedResults.StatusCode(500);
            }
            
            httpContext.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken,
                new CookieOptions() { HttpOnly = false });
            
            return TypedResults.NoContent();
        });
    }
}
