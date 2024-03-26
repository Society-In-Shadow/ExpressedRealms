using ExpressedRealms.DB;
using ExpressedRealms.Server.EndPoints.PlayerEndpoints.DTOs;
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
                        .ToListAsync();

                    return TypedResults.Ok(expressions);
                }
            )
            .RequireAuthorization();
    }
}
