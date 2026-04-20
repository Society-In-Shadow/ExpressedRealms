using ExpressedRealms.Characters.API.ProficiencyEndPoints.Responses;
using ExpressedRealms.Characters.Repository.Proficiencies;
using ExpressedRealms.Characters.Repository.Proficiencies.DTOs;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;
using ModifierDescription = ExpressedRealms.Characters.API.ProficiencyEndPoints.Responses.ModifierDescription;

namespace ExpressedRealms.Characters.API.ProficiencyEndPoints;

internal static class ProficiencyEndPoints
{
    internal static void AddProficiencyEndPoints(this WebApplication app)
    {
        var endpointGroup = app.MapGroup("proficiencies")
            .AddFluentValidationAutoValidation()
            .WithTags("Proficiencies");

        endpointGroup
            .MapGet(
                "{characterId}",
                async Task<Results<NotFound, Ok<BaseProficiencyResponse>>> (
                    int characterId,
                    IProficiencyRepository repository
                ) =>
                {
                    var results = await repository.GetBasicProficiencies(characterId);

                    if (results.HasNotFound(out var notFound))
                        return notFound;

                    return TypedResults.Ok(
                        new BaseProficiencyResponse()
                        {
                            Offensive = results
                                .Value.Where(x => x.Type == "Offensive")
                                .Select(SelectProficiencies())
                                .ToList(),
                            Defensive = results
                                .Value.Where(x => x.Type == "Defensive")
                                .Select(SelectProficiencies())
                                .ToList(),
                            Secondary = results
                                .Value.Where(x => x.Type == "Secondary")
                                .Select(SelectProficiencies())
                                .ToList(),
                        }
                    );
                }
            )
            .WithSummary("Returns all basic proficiencies for the given character")
            .RequireAuthorization();
    }

    private static Func<ProficiencyDto, ProficienciesDto> SelectProficiencies()
    {
        return x => new ProficienciesDto()
        {
            Id = x.Id,
            Value = x.Value,
            Name = x.Name,
            AppliedModifiers = x
                .AppliedModifiers.Select(y => new ModifierDescription()
                {
                    Value = y.Value,
                    Name = y.Name,
                })
                .ToList(),
        };
    }
}
