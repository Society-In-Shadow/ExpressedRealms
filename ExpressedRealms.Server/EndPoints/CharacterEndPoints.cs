using ExpressedRealms.DB;
using ExpressedRealms.Server.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Server.EndPoints;

internal static class CharacterEndPoints
{
    internal static void AddCharacterEndPoints(this WebApplication app)
    {
        app.MapGet("/characters", [Authorize] async (ExpressedRealmsDbContext dbContext, HttpContext http) =>
            {
                return await dbContext.Characters
                    .Where(x => x.Player.UserId == http.User.GetUserId()).ToListAsync();
            })
            .WithName("Characters")
            .WithOpenApi()
            .RequireAuthorization();
    }
}
