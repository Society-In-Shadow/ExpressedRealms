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
        var endpointGroup = app.MapGroup("characters")
            .AddFluentValidationAutoValidation()
            .WithTags("Characters")
            .WithOpenApi();

        endpointGroup
            .MapGet(
                "",
                [Authorize]
                async (ExpressedRealmsDbContext dbContext, HttpContext http) =>
                {
                    var characters = await dbContext
                        .Characters.Where(x => x.Player.UserId == http.User.GetUserId())
                        .ToListAsync();

                    return TypedResults.Ok(
                        characters
                            .Select(x => new CharacterListDTO()
                            {
                                Id = x.Id.ToString(),
                                Name = x.Name,
                                Background = x.Background
                            })
                            .ToList()
                    );
                        .Select(x => new CharacterListDTO()
                        {
                            Id = x.Id.ToString(),
                            Name = x.Name,
                            ShortDescription = x.Background,
                            Expression = x.Expression.Name
                        })
                        .ToListAsync();

                    return TypedResults.Ok(characters);
                }
            )
            .WithName("Characters")
            .RequireAuthorization();

        endpointGroup
            .MapGet(
                "{id}",
                [Authorize]
                async Task<Results<NotFound, Ok<CharacterDTO>>> (
                    int id,
                    ExpressedRealmsDbContext dbContext,
                    HttpContext http
                ) =>
                {
                    var character = await dbContext
                        .Characters.Where(x =>
                            x.Id == id && x.Player.UserId == http.User.GetUserId()
                        )
                        .Select(x => new CharacterDTO()
                        {
                            Name = x.Name,
                            Background = x.Background,
                            Expression = x.Expression.Name
                        })
                        .FirstOrDefaultAsync();

                    if (character is null)
                        return TypedResults.NotFound();

                    return TypedResults.Ok(character);
                }
            )
            .RequireAuthorization();

        endpointGroup
            .MapPost(
                "",
                async (
                    CreateCharacterDTO dto,
                    ExpressedRealmsDbContext dbContext,
                    HttpContext http
                ) =>
                {
                    var playerId = await dbContext
                        .Players.Where(x => x.UserId == http.User.GetUserId())
                        .Select(x => x.Id)
                        .FirstAsync();

                    var newCharacter = new Character()
                    {
                        PlayerId = playerId,
                        Name = dto.Name,
                        Background = dto.Background,
                        ExpressionId = dto.ExpressionId
                    };

                    dbContext.Characters.Add(newCharacter);

                    await dbContext.SaveChangesAsync();

                    return TypedResults.Created("/characters", newCharacter.Id);
                }
            )
            .RequireAuthorization();

        endpointGroup
            .MapDelete(
                "{id}",
                async Task<Results<NotFound, NoContent>> (
                    int id,
                    ExpressedRealmsDbContext dbContext,
                    HttpContext http
                ) =>
                {
                    var character = await dbContext.Characters.FirstOrDefaultAsync(x =>
                        x.Id == id && x.Player.UserId == http.User.GetUserId()
                    );

                    if (character is null || character.IsDeleted)
                    {
                        return TypedResults.NotFound();
                    }

                    character.SoftDelete();
                    await dbContext.SaveChangesAsync();

                    return TypedResults.NoContent();
                }
            )
            .RequireAuthorization();

        endpointGroup
            .MapPut(
                "",
                async Task<Results<NotFound, NoContent>> (
                    EditCharacterDTO dto,
                    ExpressedRealmsDbContext dbContext,
                    HttpContext http
                ) =>
                {
                    var character = await dbContext.Characters.FirstOrDefaultAsync(x =>
                        x.Id == dto.Id && x.Player.UserId == http.User.GetUserId()
                    );

                    if (character is null)
                        return TypedResults.NotFound();

                    character.Name = dto.Name;
                    character.Background = dto.Background;

                    await dbContext.SaveChangesAsync();

                    return TypedResults.NoContent();
                }
            )
            .RequireAuthorization();
    }
}
