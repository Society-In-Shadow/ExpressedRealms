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
        endpointGroup
            .MapGet(
                "/antiforgeryToken",
                Results<NoContent, StatusCodeHttpResult> (
                    IAntiforgery antiforgery,
                    HttpContext httpContext,
                    ClaimsPrincipal user
                ) =>
                {
                    var tokens = antiforgery.GetAndStoreTokens(httpContext);

                    if (tokens.RequestToken is null)
                    {
                        app.Logger.LogCritical("The anti-forgery token was not generated.");
                        return TypedResults.StatusCode(500);
                    }

                    return TypedResults.NoContent();
                }
            )
            .WithSummary("Cookie Based Anti-forgery Token for SPA")
            .WithDescription(
                """
                  Since the web api isn't handling the html, we can't use @Html.AntiForgeryToken() on each form.
                  Instead, we need to keep track of that inside a cookie.  You will need to call this endpoint
                  on initial load of the page, as well as get a new one immediately after logging in, to prevent
                  some attacks.
                """
            );
    }
}
