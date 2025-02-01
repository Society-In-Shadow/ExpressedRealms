using ExpressedRealms.DB;
using ExpressedRealms.Repositories.Characters;
using ExpressedRealms.Repositories.Characters.DTOs;
using ExpressedRealms.Repositories.Characters.Skills;
using ExpressedRealms.Repositories.Characters.Skills.DTOs;
using ExpressedRealms.Repositories.Characters.Stats;
using ExpressedRealms.Repositories.Characters.Stats.DTOs;
using ExpressedRealms.Repositories.Characters.Stats.Enums;
using ExpressedRealms.Repositories.Expressions.Expressions;
using ExpressedRealms.Server.EndPoints.CharacterEndPoints.DTOs;
using ExpressedRealms.Server.EndPoints.CharacterEndPoints.Requests;
using ExpressedRealms.Server.EndPoints.CharacterEndPoints.Responses;
using ExpressedRealms.Server.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;
using EditStatDTO = ExpressedRealms.Server.EndPoints.CharacterEndPoints.StatDTOs.EditStatDTO;
using SingleStatInfo = ExpressedRealms.Server.EndPoints.CharacterEndPoints.StatDTOs.SingleStatInfo;
using SmallStatInfo = ExpressedRealms.Server.EndPoints.CharacterEndPoints.StatDTOs.SmallStatInfo;

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
                async (ICharacterRepository repository) =>
                    TypedResults.Ok(await repository.GetCharactersAsync())
            )
            .RequireAuthorization();

        endpointGroup
            .MapGet(
                "options",
                [Authorize]
                async (ExpressedRealmsDbContext dbContext, HttpContext http) =>
                {
                    var expressions = await dbContext
                        .Expressions.AsNoTracking()
                        .Where(x => x.PublishStatusId == (int)PublishTypes.Published)
                        .Select(x => new CharacterOptionExpression()
                        {
                            Id = x.Id,
                            Name = x.Name,
                            ShortDescription = x.ShortDescription,
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
                "FactionOptions/{expressionId}",
                [Authorize]
                async Task<Results<NotFound, Ok<List<FactionOptionResponse>>>> (
                    int expressionId,
                    ExpressedRealmsDbContext dbContext,
                    HttpContext http,
                    CancellationToken cancellationToken
                ) =>
                {
                    var isValidExpression = await dbContext.Expressions.AnyAsync(
                        x =>
                            x.Id == expressionId
                            && x.PublishStatusId == (int)PublishTypes.Published,
                        cancellationToken
                    );

                    if (!isValidExpression)
                    {
                        return TypedResults.NotFound();
                    }

                    var factions = await dbContext
                        .ExpressionSections.Where(x =>
                            x.ExpressionId == expressionId
                            && x.SectionTypeId == (int)ExpressionSectionType.FactionType
                        )
                        .Select(x => new FactionOptionResponse(x.Id, x.Name, x.Content))
                        .ToListAsync(cancellationToken);

                    return TypedResults.Ok(factions);
                }
            )
            .WithSummary("Returns info needed for selecting a faction for character create")
            .WithDescription("Returns info needed for selecting a faction for character create.")
            .RequireAuthorization();

        endpointGroup
            .MapGet(
                "{characterId}/factionOptions",
                [Authorize]
                async Task<Results<NotFound, Ok<List<FactionOptionResponse>>>> (
                    int characterId,
                    ExpressedRealmsDbContext dbContext,
                    HttpContext http,
                    CancellationToken cancellationToken
                ) =>
                {
                    var character = await dbContext.Characters.FirstOrDefaultAsync(x =>
                        x.Id == characterId && x.Player.UserId == http.User.GetUserId()
                    );

                    if (character is null || character.IsDeleted)
                    {
                        return TypedResults.NotFound();
                    }

                    var factions = await dbContext
                        .ExpressionSections.Where(x =>
                            x.ExpressionId == character.ExpressionId
                            && x.SectionTypeId == (int)ExpressionSectionType.FactionType
                        )
                        .Select(x => new FactionOptionResponse(x.Id, x.Name, x.Content))
                        .ToListAsync(cancellationToken);

                    return TypedResults.Ok(factions);
                }
            )
            .WithSummary("Returns info needed for selecting a faction on edit character.")
            .WithDescription("Returns info needed for selecting a faction on edit character.")
            .RequireAuthorization();

        endpointGroup
            .MapGet(
                "{id}",
                [Authorize]
                async Task<Results<NotFound, Ok<CharacterEditResponse>>> (
                    int id,
                    ICharacterRepository repository
                ) =>
                {
                    var result = await repository.GetCharacterInfoAsync(id);

                    if (result.HasNotFound(out var notFound))
                        return notFound;
                    result.ThrowIfErrorNotHandled();

                    return TypedResults.Ok(new CharacterEditResponse(result.Value));
                }
            )
            .RequireAuthorization();

        endpointGroup
            .MapPost(
                "",
                async Task<Results<Created<int>, ValidationProblem>> (
                    CreateCharacterRequest request,
                    ICharacterRepository repository
                ) =>
                {
                    var result = await repository.CreateCharacterAsync(
                        new AddCharacterDto()
                        {
                            Name = request.Name,
                            Background = request.Background,
                            ExpressionId = request.ExpressionId,
                            FactionId = request.FactionId,
                        }
                    );

                    if (result.HasValidationError(out var validationProblem))
                        return validationProblem;
                    result.ThrowIfErrorNotHandled();

                    return TypedResults.Created("/characters", result.Value);
                }
            )
            .RequireAuthorization();

        endpointGroup
            .MapDelete(
                "{id}",
                async Task<Results<NotFound, NoContent, StatusCodeHttpResult>> (
                    int id,
                    ICharacterRepository repository
                ) =>
                {
                    var status = await repository.DeleteCharacterAsync(id);

                    if (status.HasNotFound(out var notFound))
                        return notFound;
                    if (status.HasBeenDeletedAlready(out var deletedAlready))
                        return deletedAlready;
                    status.ThrowIfErrorNotHandled();

                    return TypedResults.NoContent();
                }
            )
            .RequireAuthorization();

        endpointGroup
            .MapPut(
                "",
                async Task<Results<NotFound, NoContent, ValidationProblem>> (
                    EditCharacterRequest dto,
                    ICharacterRepository repository
                ) =>
                {
                    var status = await repository.UpdateCharacterAsync(
                        new EditCharacterDto()
                        {
                            Name = dto.Name,
                            Background = dto.Background,
                            FactionId = dto.FactionId,
                            Id = dto.Id,
                        }
                    );

                    if (status.HasNotFound(out var notFound))
                        return notFound;
                    if (status.HasValidationError(out var validationProblem))
                        return validationProblem;
                    status.ThrowIfErrorNotHandled();

                    return TypedResults.NoContent();
                }
            )
            .RequireAuthorization();

        endpointGroup
            .MapGet(
                "{characterId}/stat/{statTypeId}",
                [Authorize]
                async Task<Results<NotFound, ValidationProblem, Ok<SingleStatInfo>>> (
                    int characterId,
                    StatType statTypeId,
                    ICharacterStatRepository repository
                ) =>
                {
                    var results = await repository.GetDetailedStatInfo(
                        new GetDetailedStatInfoDto(characterId, statTypeId)
                    );

                    if (results.HasNotFound(out var notFound))
                        return notFound;
                    if (results.HasValidationError(out var validationProblem))
                        return validationProblem;
                    results.ThrowIfErrorNotHandled();

                    return TypedResults.Ok(new SingleStatInfo(results.Value));
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
                async Task<Results<NotFound, NoContent, ValidationProblem, BadRequest<string>>> (
                    EditStatDTO dto,
                    ICharacterStatRepository repository
                ) =>
                {
                    var results = await repository.UpdateCharacterStat(
                        new Repositories.Characters.Stats.DTOs.EditStatDto()
                        {
                            CharacterId = dto.CharacterId,
                            LevelTypeId = dto.LevelTypeId,
                            StatTypeId = dto.StatTypeId,
                        }
                    );

                    if (results.HasNotFound(out var notFound))
                        return notFound;
                    if (results.HasValidationError(out var validationProblem))
                        return validationProblem;
                    if (results.HasInsufficientXP(out var insufficientXPMessage))
                        return insufficientXPMessage;
                    results.ThrowIfErrorNotHandled();

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
                    ICharacterStatRepository repository
                ) =>
                {
                    var results = await repository.GetAllStats(characterId);

                    if (results.HasNotFound(out var notFound))
                        return notFound;
                    results.ThrowIfErrorNotHandled();

                    return TypedResults.Ok(
                        results.Value.Select(x => new SmallStatInfo(x)).ToList()
                    );
                }
            )
            .WithSummary("Returns the info needed for displaying the small stat tiles")
            .WithDescription(
                "Returns the info needed for displaying the small stat tiles, mainly the bonus, stat name and level."
            )
            .RequireAuthorization();

        endpointGroup
            .MapGet(
                "{characterId}/skills",
                [Authorize]
                async Task<
                    Results<NotFound, ValidationProblem, Ok<List<CharacterSkillsResponse>>>
                > (int characterId, ICharacterSkillRepository repository) =>
                {
                    var results = await repository.GetCharacterSkills(characterId);

                    return TypedResults.Ok(
                        results
                            .Select(x => new CharacterSkillsResponse()
                            {
                                Description = x.Description,
                                SkillSubTypeId = x.SkillSubTypeId,
                                Name = x.Name,
                                LevelDescription = x.LevelDescription,
                                LevelId = x.LevelId,
                                LevelName = x.LevelName,
                                SkillTypeId = x.SkillTypeId,
                                Benefits = x
                                    .Benefits.Select(y => new BenefitItemResponse()
                                    {
                                        Name = y.Name,
                                        Description = y.Description,
                                        Modifier = y.Modifier,
                                        LevelId = y.LevelId,
                                    })
                                    .ToList(),
                            })
                            .ToList()
                    );
                }
            )
            .WithSummary(
                "Returns both the basic and detailed info needed for displaying the skills"
            )
            .RequireAuthorization();

        endpointGroup
            .MapGet(
                "{characterId}/skills/{skillTypeId}",
                [Authorize]
                async Task<
                    Results<NotFound, ValidationProblem, Ok<List<CharacterSkillOptionsResponse>>>
                > (int characterId, int skillTypeId, ICharacterSkillRepository repository) =>
                {
                    var results = await repository.GetSkillLevelValuesForSkillTypeId(skillTypeId);

                    return TypedResults.Ok(
                        results
                            .Select(x => new CharacterSkillOptionsResponse()
                            {
                                Description = x.Description,
                                Name = x.Name,
                                LevelId = x.LevelId,
                                SkillTypeId = x.SkillTypeId,
                                ExperienceCost = x.ExperienceCost,
                                Benefits = x
                                    .Benefits.Select(y => new BenefitItemResponse()
                                    {
                                        Name = y.Name,
                                        Description = y.Description,
                                        Modifier = y.Modifier,
                                        LevelId = y.LevelId,
                                    })
                                    .ToList(),
                            })
                            .ToList()
                    );
                }
            )
            .WithSummary("Returns all available levels for the given skill type")
            .RequireAuthorization();

        endpointGroup
            .MapPut(
                "{characterId}/skill/{statTypeId}",
                [Authorize]
                async Task<Results<NotFound, NoContent, ValidationProblem, BadRequest<string>>> (
                    EditCharacterSkillRequest dto,
                    ICharacterSkillRepository repository
                ) =>
                {
                    var results = await repository.UpdateSkillLevel(
                        new EditCharacterSkillMappingDto()
                        {
                            SkillLevelId = dto.SkillLevelId,
                            CharacterId = dto.CharacterId,
                            SkillTypeId = dto.SkillTypeId,
                        }
                    );

                    if (results.HasValidationError(out var validationProblem))
                        return validationProblem;
                    if (results.HasNotFound(out var notFound))
                        return notFound;

                    results.ThrowIfErrorNotHandled();

                    return TypedResults.NoContent();
                }
            )
            .WithSummary("Allows user to update the given skill with the provided level")
            .WithDescription("Allows user to update the given skill with the provided level")
            .RequireAuthorization();
    }
}
