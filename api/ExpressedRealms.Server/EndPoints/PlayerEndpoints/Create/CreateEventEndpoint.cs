using ExpressedRealms.Characters.Repository.Players;
using ExpressedRealms.DB;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.PlayerSetup;
using ExpressedRealms.Server.Shared.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Server.EndPoints.PlayerEndpoints.Create;

public static class CreatePlayerEndpoint
{
    public static async Task<Results<Created, ValidationProblem>> ExecuteAsync(
        [FromBody] CreatePlayerRequest request,
        [FromServices] ExpressedRealmsDbContext dbContext,
        [FromServices] IHttpContextAccessor http,
        [FromServices] IPlayerRepository playerRepository
    )
    {
        var userId = http.HttpContext!.User.GetUserId();
        var isExistingPlayer = await dbContext.Players.FirstOrDefaultAsync(x => x.UserId == userId);

        if (isExistingPlayer is not null)
            return TypedResults.Created("/player");

        var player = new Player()
        {
            Id = new Guid(),
            Name = request.Name,
            UserId = userId,
            LookupId = await playerRepository.GetUniqueLookupId(),
        };

        await dbContext.Players.AddAsync(player);
        await dbContext.SaveChangesAsync();

        return TypedResults.Created("/player");
    }
}
