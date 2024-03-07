using ExpressedRealms.DB;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Server.EndPoints;

internal static class PlayerEndpoints
{
    internal static void AddPlayerEndPoints(this WebApplication app)
    {
        app.MapGroup("player").MapGet("/isSetup",
                async (ExpressedRealmsDbContext dbContext,
                    HttpContext http) =>
                {
                    var player = await dbContext.Players.FirstOrDefaultAsync(x => x.UserId == http.User.GetUserId());
                    return player is not null;
                })
            .WithName("isSetup")
            .WithOpenApi()
            .RequireAuthorization();
    }
}
