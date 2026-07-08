using ExpressedRealms.Authentication.PermissionCollection;
using ExpressedRealms.Authentication.PermissionCollection.Configuration;
using ExpressedRealms.Characters.API.CharacterEndPoints.CopyCharacter;
using ExpressedRealms.Characters.API.CharacterEndPoints.DTOs;
using ExpressedRealms.Characters.API.CharacterEndPoints.EditCharacter;
using ExpressedRealms.Characters.API.CharacterEndPoints.EditCharacterGoFields;
using ExpressedRealms.Characters.API.CharacterEndPoints.EditCharacterOptions;
using ExpressedRealms.Characters.API.CharacterEndPoints.EditStatInfo;
using ExpressedRealms.Characters.API.CharacterEndPoints.FinalizeCharacterCreate;
using ExpressedRealms.Characters.API.CharacterEndPoints.GetArchetypesForExpression;
using ExpressedRealms.Characters.API.CharacterEndPoints.GetBreakOfDawnInfo;
using ExpressedRealms.Characters.API.CharacterEndPoints.GetCharacterGoFields;
using ExpressedRealms.Characters.API.CharacterEndPoints.GetCRB;
using ExpressedRealms.Characters.API.CharacterEndPoints.GetDetailedStatInfo;
using ExpressedRealms.Characters.API.CharacterEndPoints.GetOverallStats;
using ExpressedRealms.Characters.API.CharacterEndPoints.GetStatsForCharacter;
using ExpressedRealms.Characters.API.CharacterEndPoints.Requests;
using ExpressedRealms.Characters.API.CharacterEndPoints.Responses;
using ExpressedRealms.Characters.API.CharacterEndPoints.RetireCharacter;
using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.Repository.DTOs;
using ExpressedRealms.Characters.Repository.Skills;
using ExpressedRealms.Characters.Repository.Skills.DTOs;
using ExpressedRealms.DB;
using ExpressedRealms.Expressions.Repository.Expressions;
using ExpressedRealms.FeatureFlags;
using ExpressedRealms.Repositories.Shared.ExternalDependencies;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace ExpressedRealms.Characters.API.CharacterEndPoints;

internal static class CharacterEndPoints
{
    internal static void AddCharacterEndPoints(this WebApplication app)
    {
        var endpointGroup = app.MapGroup("characters")
            .AddFluentValidationAutoValidation()
            .WithTags("Characters");

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
                async (
                    ExpressedRealmsDbContext dbContext,
                    ICharacterRepository repository,
                    IUserContext userContext
                ) =>
                {
                    var allowedStatuses = new List<int> { (int)PublishTypes.Published };
                    if (
                        userContext.CurrentUserHasPermission(
                            Permissions.Expression.SeeBetaExpressions
                        )
                    )
                    {
                        allowedStatuses.Add((int)PublishTypes.Beta);
                    }

                    var expressions = await dbContext
                        .Expressions.AsNoTracking()
                        .Where(x =>
                            allowedStatuses.Contains(x.PublishStatusId) && x.CmsTypeId == 1
                        )
                        .Select(x => new CharacterOptionExpression()
                        {
                            Id = x.Id,
                            Name =
                                x.PublishStatusId == (int)PublishTypes.Beta
                                    ? x.Name + " (Beta)"
                                    : x.Name,
                        })
                        .OrderBy(x => x.Name)
                        .ToListAsync();

                    return TypedResults.Ok(new CharacterOptions() { Expressions = expressions });
                }
            )
            .WithSummary("Returns info needed for creating a character")
            .WithDescription("Returns info needed for creating a character.")
            .RequireAuthorization();

        endpointGroup
            .MapGet("{id}/options", EditCharacterOptionsEndpoint.Execute)
            .WithDescription("Returns info needed for creating a character.")
            .RequireAuthorization();

        endpointGroup
            .MapGet("{characterId}/dailyCheckinInfo", GetBreakOfDawnInfoEndpoint.ExecuteAsync)
            .RequireAuthorization();

        endpointGroup
            .MapPost("{characterId}/copy", CopyCharacterEndpoint.ExecuteAsync)
            .RequireFeatureToggle(ReleaseFlags.ShowArchetypeSelection)
            .RequireAuthorization();

        endpointGroup
            .MapGet("/archetypes/{id}", GetArchetypesForExpressionEndpoint.ExecuteAsync)
            .RequireFeatureToggle(ReleaseFlags.ShowArchetypeSelection)
            .RequireAuthorization();

        endpointGroup
            .MapPut("{characterId}/goFields", EditCharacterGoFieldsEndpoint.ExecuteAsync)
            .RequirePermission(Permissions.CharacterManagement.ModifyGoFields);

        endpointGroup
            .MapGet("{characterId}/goFields", GetCharacterGoFieldsEndpoint.ExecuteAsync)
            .RequirePermission(Permissions.CharacterManagement.ModifyGoFields);

        endpointGroup
            .MapGet(
                "options/{expressionId}",
                [Authorize]
                async (
                    int expressionId,
                    ExpressedRealmsDbContext dbContext,
                    HttpContext http,
                    IUserContext userContext
                ) =>
                {
                    var allowedStatuses = new List<int> { (int)PublishTypes.Published };
                    if (
                        userContext.CurrentUserHasPermission(
                            Permissions.Expression.SeeBetaExpressions
                        )
                    )
                    {
                        allowedStatuses.Add((int)PublishTypes.Beta);
                    }
                    var expressionInfo = await dbContext
                        .Expressions.AsNoTracking()
                        .Where(x =>
                            allowedStatuses.Contains(x.PublishStatusId)
                            && x.CmsTypeId == 1
                            && x.Id == expressionId
                        )
                        .Select(x => new HighLevelExpressionInfoResponse()
                        {
                            Name = x.Name,
                            Archetypes =
                                x.ExpressionSections.Where(y => y.SectionTypeId == 16)
                                    .Select(z => z.Content)
                                    .FirstOrDefault() ?? string.Empty,
                            Description =
                                x.ExpressionSections.Where(y => y.SectionTypeId == 1)
                                    .Select(z => z.Content)
                                    .FirstOrDefault() ?? string.Empty,
                            Background =
                                x.ExpressionSections.Where(y => y.SectionTypeId == 2)
                                    .Select(z => z.Content)
                                    .FirstOrDefault() ?? string.Empty,
                        })
                        .FirstAsync();

                    return TypedResults.Ok(expressionInfo);
                }
            )
            .WithSummary("Returns high level expression info to be displayed in the wizard")
            .RequireAuthorization();

        endpointGroup
            .MapGet(
                "ProgressionOptions/{expressionId}",
                [Authorize]
                async Task<Results<NotFound, Ok<List<PowerPathOptionResponse>>>> (
                    int expressionId,
                    ExpressedRealmsDbContext dbContext,
                    ICharacterRepository characterRepository,
                    HttpContext http,
                    CancellationToken cancellationToken
                ) =>
                {
                    var isValidExpression = await characterRepository.ExpressionExistsAsync(
                        expressionId
                    );

                    if (!isValidExpression)
                    {
                        return TypedResults.NotFound();
                    }

                    var factions = await dbContext
                        .ProgressionPath.Where(x => x.ExpressionId == expressionId)
                        .Select(x => new PowerPathOptionResponse(x.Id, x.Name, x.Description))
                        .ToListAsync(cancellationToken);

                    return TypedResults.Ok(factions);
                }
            )
            .WithSummary(
                "Returns info needed for selecting a progression path for character create"
            )
            .WithDescription(
                "Returns info needed for selecting a progression path for character create."
            )
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
            .MapGet("{id}/overallexperience", GetOverallStatsEndpoint.Execute)
            .RequireAuthorization();

        endpointGroup
            .MapGet("{characterId}/getcrb", GetExpressionBookletEndpoint.Execute)
            .RequireAuthorization();

        endpointGroup
            .MapPut("{lookupId}/retire", RetireCharacterEndpoint.Execute)
            .RequirePermission(Permissions.CharacterManagement.Retire);

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
                            ExpressionId = request.ExpressionId,
                            IsArchetype = request.IsArchetype,
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
            .MapPut("{id}/finalizeCharacterCreate", FinalizeCharacterCreateEndpoint.Execute)
            .RequireAuthorization();

        endpointGroup.MapPut("{id}", EditCharacterEndpoint.Execute).RequireAuthorization();

        endpointGroup
            .MapGet(
                "{characterId}/stat/{statTypeId}",
                GetDetailedStatInfoForCharacterEndpoint.ExecuteAsync
            )
            .RequireAuthorization();

        endpointGroup
            .MapPut(
                "{characterId}/stat/{statTypeId}",
                EditStatInfoForCharacterEndpoint.ExecuteAsync
            )
            .RequireAuthorization();

        endpointGroup
            .MapGet("{characterId}/stats", GetStatsForCharacterEndpoint.ExecuteAsync)
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
                > (
                    int characterId,
                    ICharacterSkillRepository repository,
                    ICharacterRepository characterRepository
                ) =>
                {
                    var results = await repository.GetCharacterSkills(characterId);

                    if (!await characterRepository.CharacterExistsAsync(characterId))
                    {
                        return TypedResults.NotFound();
                    }

                    return TypedResults.Ok(
                        results
                            .Select(x => new CharacterSkillsResponse()
                            {
                                Description = x.Description,
                                SkillSubTypeId = x.SkillSubTypeId,
                                Name = x.Name,
                                SkillTypeId = x.SkillTypeId,
                                LevelId = x.LevelId,
                                LevelName = x.LevelName,
                                LevelNumber = x.LevelNumber,
                                XP = x.XP,
                                TotalXp = x.TotalXp,
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
                                LevelNumber = x.LevelNumber,
                                Benefits = x
                                    .Benefits.Select(y => new BenefitItemResponse()
                                    {
                                        Name = y.Name,
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
                    if (results.HasInsufficientXP(out var insufficientXPMessage))
                        return insufficientXPMessage;
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
