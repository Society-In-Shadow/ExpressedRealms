using ExpressedRealms.DB;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.PlayerSetup;
using ExpressedRealms.Server.EndPoints.PlayerEndpoints.DTOs;
using ExpressedRealms.Server.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace ExpressedRealms.Server.EndPoints.PlayerEndpoints;

internal static class PlayerEndpoints
{
    internal static void AddPlayerEndPoints(this WebApplication app)
    {
        var endpointGroup = app.MapGroup("player")
            .AddFluentValidationAutoValidation()
            .WithTags("Player")
            .WithOpenApi();

        endpointGroup
            .MapGet(
                "/playerName",
                async (ExpressedRealmsDbContext dbContext, HttpContext http) =>
                {
                    var player = await dbContext.Players.FirstOrDefaultAsync(x =>
                        x.UserId == http.User.GetUserId()
                    );
                    return TypedResults.Ok(new PlayerNameDTO() { Name = player?.Name });
                }
            )
            .WithSummary("Grab Player Name, Redirect to Player Creation if not filled in.")
            .WithDescription(
                """
                This will grab the user name for upper right hand corner of the app on the user profile menu.
                If it's filled in, it indicates that the user has already setup their profile.  If it's 
                null, then the app will redirect the user to the create profile setup page, so they can get 
                that filled in.
                """
            )
            .RequireAuthorization();

        endpointGroup
            .MapGet(
                "",
                async Task<Results<NotFound, Ok<PlayerDTO>>> (
                    ExpressedRealmsDbContext dbContext,
                    HttpContext http
                ) =>
                {
                    var player = await dbContext.Players.FirstOrDefaultAsync(x =>
                        x.UserId == http.User.GetUserId()
                    );

                    if (player is null)
                        return TypedResults.NotFound();

                    return TypedResults.Ok(new PlayerDTO() { Name = player.Name });
                }
            )
            .WithSummary("No Id Required, this is specific to the User")
            .WithDescription(
                """
                The user automatically keeps track of the user information.  So you will never need an id 
                to grab this information.  Will throw an error if they do not have the player information
                setup.  You should be using the playerName endpoint instead.
                """
            )
            .RequireAuthorization();

        endpointGroup
            .MapPost(
                "",
                async (PlayerDTO playerDto, ExpressedRealmsDbContext dbContext, HttpContext http) =>
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
                            UserId = http.User.GetUserId(),
                        };

                        await dbContext.Players.AddAsync(player);
                        await dbContext.SaveChangesAsync();
                    }

                    return TypedResults.Created("/player");
                }
            )
            .WithSummary("Will only be called once upon initial login")
            .WithDescription(
                """
                This will be called upon initial login, after they fill in the details.  Any further calls 
                to it will only return created without updating the user.
                """
            )
            .RequireAuthorization();

        endpointGroup
            .MapPut(
                "",
                async (PlayerDTO playerDto, ExpressedRealmsDbContext dbContext, HttpContext http) =>
                {
                    var existingPlayer = await dbContext.Players.FirstAsync(x =>
                        x.UserId == http.User.GetUserId()
                    );

                    existingPlayer.Name = playerDto.Name;

                    await dbContext.SaveChangesAsync();

                    return TypedResults.NoContent();
                }
            )
            .RequireAuthorization();
    }
}
