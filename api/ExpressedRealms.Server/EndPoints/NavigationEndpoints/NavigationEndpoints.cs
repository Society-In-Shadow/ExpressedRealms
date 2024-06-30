using ExpressedRealms.DB;
using ExpressedRealms.Server.EndPoints.NavigationEndpoints.Responses;
using ExpressedRealms.Server.EndPoints.PlayerEndpoints.DTOs;
using ExpressedRealms.Server.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace ExpressedRealms.Server.EndPoints.PlayerEndpoints;

internal static class NavigationEndpoints
{
    internal static void AddNavigationEndpoints(this WebApplication app)
    {
        var endpointGroup = app.MapGroup("navMenu")
            .AddFluentValidationAutoValidation()
            .WithTags("Nav Menu")
            .WithOpenApi();

        endpointGroup
            .MapGet(
                "/expressions",
                async (ExpressedRealmsDbContext dbContext) =>
                {
                    var expressions = await dbContext
                        .Expressions.Select(x => new ExpressionMenuItem()
                        {
                            Name = x.Name,
                            Id = x.Id,
                            ShortDescription = x.ShortDescription,
                            NavMenuImage = x.NavMenuImage
                        })
                        .OrderBy(x => x.Name)
                        .ToListAsync();

                    return TypedResults.Ok(expressions);
                }
            )
            .RequireAuthorization();

        endpointGroup
            .MapGet(
                "characters",
                [Authorize]
                async (ExpressedRealmsDbContext dbContext, HttpContext http) =>
                {
                    var characters = await dbContext
                        .Characters.Where(x => x.Player.UserId == http.User.GetUserId())
                        .Select(x => new CharacterNavResponse(
                            x.Id,
                            x.Name,
                            x.Expression.Name,
                            x.Background.Substring(0, 51) ?? ""
                        ))
                        .ToListAsync();

                    characters.ForEach(x =>
                    {
                        if (x.Background.Length > 50)
                        {
                            x.Background = x.Background.Substring(0, 50) + "...";
                        }
                    });

                    return TypedResults.Ok(characters);
                }
            )
            .WithSummary(
                "Returns a simplified version of the characters for display in the nav menu."
            )
            .WithDescription(
                "Returns the all characters with their name, expression, and a truncated background (50 characters + '...',  or less)."
            )
            .RequireAuthorization();
    }
}
