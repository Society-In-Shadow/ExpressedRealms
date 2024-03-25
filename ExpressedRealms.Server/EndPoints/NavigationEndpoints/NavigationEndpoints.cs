using ExpressedRealms.DB;
using ExpressedRealms.DB.UserProfile.PlayerDBModels;
using ExpressedRealms.Server.EndPoints.PlayerEndpoints.DTOs;
using ExpressedRealms.Server.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
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
                async (ExpressedRealmsDbContext dbContext, HttpContext http) =>
                {
                    var expressions = new List<ExpressionMenuItem>()
                    {
                        new()
                        {
                            Id = 1,
                            Name = "Vampyres",
                            ShortDescription = "Spooky Capes",
                            NavMenuImage = ""
                        },
                        new()
                        {
                            Id = 2,
                            Name = "Sorcerers",
                            ShortDescription = "Spooky Magic",
                            NavMenuImage = ""
                        }
                    };
                        
                    return TypedResults.Ok(expressions);
                }
            )
            .RequireAuthorization();

    }
}
