using System.ComponentModel.DataAnnotations;
using ExpressedRealms.DB;
using ExpressedRealms.DB.Characters;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.Server.EndPoints.CharacterEndPoints.DTOs;
using ExpressedRealms.Server.EndPoints.CharacterEndPoints.StatDTOs;
using ExpressedRealms.Server.EndPoints.DTOs;
using ExpressedRealms.Server.Extensions;
using FluentValidation;
using FluentValidation.Results;
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
                        .Select(x => new CharacterListDTO()
                        {
                            Id = x.Id.ToString(),
                            Name = x.Name,
                            Background = x.Background,
                            Expression = x.Expression.Name
                        })
                        .ToListAsync();

                    return TypedResults.Ok(characters);
                }
            )
            .RequireAuthorization();

        endpointGroup
            .MapGet(
                "options",
                [Authorize]
                async (ExpressedRealmsDbContext dbContext, HttpContext http) =>
                {
                    var expressions = await dbContext
                        .Expressions.Select(x => new CharacterOptionExpression()
                        {
                            Id = x.Id,
                            Name = x.Name,
                            ShortDescription = x.ShortDescription
                        })
                        .ToListAsync();

                    return TypedResults.Ok(new CharacterOptions() { Expressions = expressions });
                }
            )
            .WithSummary("Returns info needed for creating a character")
            .WithDescription("Returns info needed for creating a character.")
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

        endpointGroup
            .MapGet(
                "{characterId}/stat/{statTypeId}",
                [Authorize]
                async Task<
                    Results<NotFound, BadRequest<List<ValidationFailure>>, Ok<SingleStatInfo>>
                > (
                    int characterId,
                    StatType statTypeId,
                    IValidator<EditStatRequest> validator,
                    ExpressedRealmsDbContext dbContext,
                    HttpContext http
                ) =>
                {
                    var result = await validator.ValidateAsync(
                        new EditStatRequest(characterId, statTypeId)
                    );
                    if (!result.IsValid)
                        return TypedResults.BadRequest<List<ValidationFailure>>(result.Errors);

                    var character = await dbContext
                        .Characters.Where(x => x.Id == characterId)
                        .Select(x => new
                        {
                            AgilityId = x.AgilityId,
                            ConstitutionId = x.ConstitutionId,
                            DexterityId = x.DexterityId,
                            StrengthId = x.StrengthId,
                            IntelligenceId = x.IntelligenceId,
                            WillpowerId = x.WillpowerId,
                            AvailableXP = x.StatExperiencePoints
                                - (
                                    x.AgilityStatLevel.TotalXPCost
                                    + x.ConstitutionStatLevel.TotalXPCost
                                    + x.DexterityStatLevel.TotalXPCost
                                    + x.StrengthStatLevel.TotalXPCost
                                    + x.IntelligenceStatLevel.TotalXPCost
                                    + x.WillpowerStatLevel.TotalXPCost
                                )
                        })
                        .FirstOrDefaultAsync();

                    if (character is null)
                        return TypedResults.NotFound();

                    var statLevelId = statTypeId switch
                    {
                        StatType.Agility => character.AgilityId,
                        StatType.Constitution => character.ConstitutionId,
                        StatType.Dexterity => character.DexterityId,
                        StatType.Strength => character.StrengthId,
                        StatType.Intelligence => character.IntelligenceId,
                        StatType.Willpower => character.WillpowerId,
                    };

                    var statInfo = await dbContext
                        .StateTypes.Where(x => x.Id == (byte)statTypeId)
                        .Select(x => new SingleStatInfo()
                        {
                            Id = (StatType)x.Id,
                            Name = x.Name,
                            Description = x.Description,
                            StatLevel = statLevelId,
                            AvailableXP = character.AvailableXP,
                            StatLevelInfo = x
                                .StatDescriptionMappings.Where(y => y.StatLevelId == statLevelId)
                                .Select(y => new StatDetails()
                                {
                                    Level = y.StatLevelId,
                                    XP = y.StatLevel.XPCost,
                                    Bonus = y.StatLevel.Bonus,
                                    Description = y.ReasonableExpectation,
                                    TotalXP = y.StatLevel.TotalXPCost
                                })
                                .First()
                        })
                        .FirstAsync();

                    return TypedResults.Ok(statInfo);
                }
            )
            .WithSummary("This returns the detailed information for the given stat")
            .WithDescription(
                "This returns the detailed information for the given stat.  This will include the full name, what it does, current level, bonus, xp, and description."
            )
            .RequireAuthorization();

        endpointGroup
            .MapPut(
                "{characterId}/stat/{statTypeId}",
                [Authorize]
                async Task<Results<NotFound, NoContent, BadRequest<string>>> (
                    EditStatDTO dto,
                    ExpressedRealmsDbContext dbContext,
                    HttpContext http
                ) =>
                {
                    var character = await dbContext
                        .Characters.Where(x => x.Id == dto.CharacterId)
                        .Include(x => x.AgilityStatLevel)
                        .Include(x => x.StrengthStatLevel)
                        .Include(x => x.ConstitutionStatLevel)
                        .Include(x => x.DexterityStatLevel)
                        .Include(x => x.IntelligenceStatLevel)
                        .Include(x => x.WillpowerStatLevel)
                        .FirstOrDefaultAsync();

                    if (character is null)
                        return TypedResults.NotFound();

                    var availableXp = await dbContext
                        .Characters.Where(x => x.Id == dto.CharacterId)
                        .Select(x =>
                            x.StatExperiencePoints
                            - (
                                x.AgilityStatLevel.TotalXPCost
                                + x.ConstitutionStatLevel.TotalXPCost
                                + x.DexterityStatLevel.TotalXPCost
                                + x.StrengthStatLevel.TotalXPCost
                                + x.IntelligenceStatLevel.TotalXPCost
                                + x.WillpowerStatLevel.TotalXPCost
                            )
                        )
                        .FirstOrDefaultAsync();

                    var oldTotalXpCost = dto.StatTypeId switch
                    {
                        StatType.Agility => character.AgilityStatLevel.TotalXPCost,
                        StatType.Strength => character.StrengthStatLevel.TotalXPCost,
                        StatType.Constitution => character.ConstitutionStatLevel.TotalXPCost,
                        StatType.Dexterity => character.DexterityStatLevel.TotalXPCost,
                        StatType.Intelligence => character.IntelligenceStatLevel.TotalXPCost,
                        StatType.Willpower => character.WillpowerStatLevel.TotalXPCost,
                        _ => throw new ArgumentOutOfRangeException()
                    };

                    var newTotalXpCost = await dbContext
                        .StatLevels.Where(x => x.Id == dto.LevelTypeId)
                        .Select(x => x.TotalXPCost)
                        .FirstAsync();

                    if (availableXp < newTotalXpCost - oldTotalXpCost)
                    {
                        return TypedResults.BadRequest<string>(
                            "You don't have enough XP to select that level.  You have "
                                + availableXp
                                + " points available.  You tried to spend "
                                + (newTotalXpCost - oldTotalXpCost)
                                + " points."
                        );
                    }

                    switch (dto.StatTypeId)
                    {
                        case StatType.Agility:
                            character.AgilityId = dto.LevelTypeId;
                            break;
                        case StatType.Constitution:
                            character.ConstitutionId = dto.LevelTypeId;
                            break;
                        case StatType.Dexterity:
                            character.DexterityId = dto.LevelTypeId;
                            break;
                        case StatType.Strength:
                            character.StrengthId = dto.LevelTypeId;
                            break;
                        case StatType.Intelligence:
                            character.IntelligenceId = dto.LevelTypeId;
                            break;
                        case StatType.Willpower:
                            character.WillpowerId = dto.LevelTypeId;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    await dbContext.SaveChangesAsync();

                    return TypedResults.NoContent();
                }
            )
            .WithSummary("Allows user to update the given state with the provided level")
            .WithDescription("Allows user to update the given state with the provided level")
            .RequireAuthorization();

        endpointGroup
            .MapGet(
                "{characterId}/stats",
                [Authorize]
                async Task<Results<NotFound, Ok<List<SmallStatInfo>>>> (
                    int characterId,
                    ExpressedRealmsDbContext dbContext,
                    HttpContext http
                ) =>
                {
                    var character = await dbContext
                        .Characters.Include(x => x.AgilityStatLevel)
                        .Include(x => x.ConstitutionStatLevel)
                        .Include(x => x.DexterityStatLevel)
                        .Include(x => x.StrengthStatLevel)
                        .Include(x => x.IntelligenceStatLevel)
                        .Include(x => x.WillpowerStatLevel)
                        .FirstOrDefaultAsync(x => x.Id == characterId);

                    var statTypes = await dbContext.StateTypes.ToListAsync();
                    if (character is null)
                        return TypedResults.NotFound();

                    var characterStats = new List<SmallStatInfo>()
                    {
                        new()
                        {
                            StatTypeId = StatType.Agility,
                            Bonus = character.AgilityStatLevel.Bonus,
                            Level = character.AgilityStatLevel.Id,
                            ShortName = statTypes
                                .First(x => x.Id == (byte)StatType.Agility)
                                .ShortName
                        },
                        new()
                        {
                            StatTypeId = StatType.Constitution,
                            Bonus = character.ConstitutionStatLevel.Bonus,
                            Level = character.ConstitutionStatLevel.Id,
                            ShortName = statTypes
                                .First(x => x.Id == (byte)StatType.Constitution)
                                .ShortName
                        },
                        new()
                        {
                            StatTypeId = StatType.Dexterity,
                            Bonus = character.DexterityStatLevel.Bonus,
                            Level = character.DexterityStatLevel.Id,
                            ShortName = statTypes
                                .First(x => x.Id == (byte)StatType.Dexterity)
                                .ShortName
                        },
                        new()
                        {
                            StatTypeId = StatType.Strength,
                            Bonus = character.StrengthStatLevel.Bonus,
                            Level = character.StrengthStatLevel.Id,
                            ShortName = statTypes
                                .First(x => x.Id == (byte)StatType.Strength)
                                .ShortName
                        },
                        new()
                        {
                            StatTypeId = StatType.Intelligence,
                            Bonus = character.IntelligenceStatLevel.Bonus,
                            Level = character.IntelligenceStatLevel.Id,
                            ShortName = statTypes
                                .First(x => x.Id == (byte)StatType.Intelligence)
                                .ShortName
                        },
                        new()
                        {
                            StatTypeId = StatType.Willpower,
                            Bonus = character.WillpowerStatLevel.Bonus,
                            Level = character.WillpowerStatLevel.Id,
                            ShortName = statTypes
                                .First(x => x.Id == (byte)StatType.Willpower)
                                .ShortName
                        }
                    };

                    return TypedResults.Ok(characterStats);
                }
            )
            .WithSummary("Returns the info needed for displaying the small stat tiles")
            .WithDescription(
                "Returns the info needed for displaying the small stat tiles, mainly the bonus, stat name and level."
            )
            .RequireAuthorization();
    }
}
