using ExpressedRealms.DB;
using ExpressedRealms.DB.Characters;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.Server.EndPoints.CharacterEndPoints.DTOs;
using ExpressedRealms.Server.EndPoints.DTOs;
using ExpressedRealms.Server.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace ExpressedRealms.Server.EndPoints.CharacterEndPoints;

internal static class CharacterEndPoints
{
    internal static void AddCharacterEndPoints(this WebApplication app)
    {
        var endpointGroup = app.MapGroup("characters").AddFluentValidationAutoValidation();

        endpointGroup
            .MapGet("", [Authorize] async (ExpressedRealmsDbContext dbContext, HttpContext http) =>
            {
                var characters =  await dbContext.Characters
                    .Where(x => x.Player.UserId == http.User.GetUserId()).ToListAsync();

                return TypedResults.Ok(characters.Select(x => new CharacterListDTO()
                {
                    Id = x.Id.ToString(),
                    Name = x.Name,
                    Background = x.Background
                }).ToList());
            })
            .WithName("Characters")
            .WithOpenApi()
            .RequireAuthorization();
        
        endpointGroup
            .MapGet("{id}", [Authorize] async Task<Results<NotFound, Ok<CharacterDTO>>>(int id, ExpressedRealmsDbContext dbContext, HttpContext http) =>
            {
                var character = await dbContext.Characters
                    .Where(x => x.Id == id && x.Player.UserId == http.User.GetUserId()).FirstOrDefaultAsync();

                if (character is null)
                    return TypedResults.NotFound();

                return TypedResults.Ok(new CharacterDTO()
                {
                    Name = character.Name,
                    Background = character.Background
                });
            })
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

                return TypedResults.Created("/characters", newCharacter.Id);
            })
            .WithOpenApi()
            .RequireAuthorization();

        endpointGroup
            .MapDelete("{id}", async Task<Results<NotFound, NoContent>> (int id, ExpressedRealmsDbContext dbContext, HttpContext http) =>
            {
                var character =
                    await dbContext.Characters.FirstOrDefaultAsync(x =>
                        x.Id == id && x.Player.UserId == http.User.GetUserId());

                if (character is null || character.IsDeleted)
                {
                    return TypedResults.NotFound();
                }
                
                character.SoftDelete();
                await dbContext.SaveChangesAsync();

                return TypedResults.NoContent();
            })
            .WithOpenApi()
            .RequireAuthorization();

        endpointGroup.MapPut("",
            async Task<Results<NotFound, NoContent>> (EditCharacterDTO dto, ExpressedRealmsDbContext dbContext, HttpContext http) =>
            {
                var character =
                    await dbContext.Characters.FirstOrDefaultAsync(x =>
                        x.Id == dto.Id && x.Player.UserId == http.User.GetUserId());

                if (character is null)
                    return TypedResults.NotFound();

                character.Name = dto.Name;
                character.Background = dto.Background;

                await dbContext.SaveChangesAsync();

                return TypedResults.NoContent();
            })
            .WithOpenApi()
            .RequireAuthorization();
    }
}
