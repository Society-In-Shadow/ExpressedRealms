using ExpressedRealms.DB;
using ExpressedRealms.DB.Characters;
using ExpressedRealms.Server.EndPoints.DTOs;
using ExpressedRealms.Server.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace ExpressedRealms.Server.EndPoints;

internal static class CharacterEndPoints
{
    internal static void AddCharacterEndPoints(this WebApplication app)
    {
        var endpointGroup = app.MapGroup("characters").AddFluentValidationAutoValidation();
        
        endpointGroup
            .MapGet("", [Authorize] async (ExpressedRealmsDbContext dbContext, HttpContext http) =>
            {
                return await dbContext.Characters
                    .Where(x => x.Player.UserId == http.User.GetUserId()).ToListAsync();
            })
            .WithName("Characters")
            .WithOpenApi()
            .RequireAuthorization();

        endpointGroup
            .MapPost("", async (CreateCharacterDTO dto, ExpressedRealmsDbContext dbContext, HttpContext http) =>
            {
                var playerId = await dbContext.Players
                    .Where(x => x.UserId == http.User.GetUserId())
                    .Select(x => x.Id)
                    .FirstAsync();

                var newCharacter = new Character()
                {
                    PlayerId = playerId,
                    Name = dto.Name,
                    Background = dto.Background
                };
                
                dbContext.Characters.Add(newCharacter);

                await dbContext.SaveChangesAsync();

                return Results.Created("/characters", newCharacter.Id);
            })
            .WithOpenApi()
            .RequireAuthorization();
    }
}
