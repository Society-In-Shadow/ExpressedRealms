using ExpressedRealms.DB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Server.EndPoints;

internal static class CharacterEndPoints
{
    internal static void AddCharacterEndPoints(this WebApplication app)
    {
        app.MapGet("/characters", [Authorize] async (ExpressedRealmsDbContext dbContext) =>
            {
                return await dbContext.Characters.ToListAsync();
            })
            .WithName("Characters")
            .WithOpenApi()
            .RequireAuthorization();
    }
}
