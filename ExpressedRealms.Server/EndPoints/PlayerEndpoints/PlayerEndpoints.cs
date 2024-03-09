using ExpressedRealms.DB;
using ExpressedRealms.DB.UserProfile.PlayerDBModels;
using ExpressedRealms.Server.EndPoints.PlayerEndpoints.DTOs;
using ExpressedRealms.Server.Extensions;
using Microsoft.EntityFrameworkCore;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace ExpressedRealms.Server.EndPoints.PlayerEndpoints;

internal static class PlayerEndpoints
{
    internal static void AddPlayerEndPoints(this WebApplication app)
    {
        var endpointGroup = app.MapGroup("player").AddFluentValidationAutoValidation();

        endpointGroup
            .MapGet(
                "/isSetup",
                async (ExpressedRealmsDbContext dbContext, HttpContext http) =>
                {
                    var player = await dbContext.Players.FirstOrDefaultAsync(x =>
                        x.UserId == http.User.GetUserId()
                    );
                    return player?.Name;
                }
            )
            .WithName("isSetup")
            .WithOpenApi()
            .RequireAuthorization();

        endpointGroup
            .MapPost(
                "/addUserProfile",
                async (
                    CreatePlayerDTO playerDto,
                    ExpressedRealmsDbContext dbContext,
                    HttpContext http
                ) =>
                {
                    var isExistingPlayer = await dbContext.Players.FirstOrDefaultAsync(x =>
                        x.UserId == http.User.GetUserId()
                    );

                    if (isExistingPlayer is null)
                    {
                        var player = new Player()
                        {
                            Id = new Guid(),
                            Name = playerDto.Name,
                            City = playerDto.City,
                            Phone = playerDto.PhoneNumber,
                            State = playerDto.State,
                            PlayerNumber = 1,
                            UserId = http.User.GetUserId()
                        };

                        await dbContext.Players.AddAsync(player);
                        await dbContext.SaveChangesAsync();
                    }

                    return Results.Created();
                }
            )
            .WithName("addUserProfile")
            .WithOpenApi()
            .RequireAuthorization();
    }
}
