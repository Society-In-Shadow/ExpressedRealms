using ExpressedRealms.Characters.API.ProficiencyEndPoints.Responses;
using ExpressedRealms.Characters.Repository.Proficiencies;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace ExpressedRealms.Characters.API.ProficiencyEndPoints;

internal static class ProficiencyEndPoints
{
    internal static void AddProficiencyEndPoints(this WebApplication app)
    {
        var endpointGroup = app.MapGroup("proficiencies")
            .AddFluentValidationAutoValidation()
            .WithTags("Proficiencies")
            .WithOpenApi();

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
                            Proficiencies = results
                                .Value.Select(x => new ProficienciesDto()
                                {
                                    Id = x.Id,
                                    Value = x.Value,
                                    Name = x.Name,
                                    Description = x.Description,
                                    Modifiers = x.Modifiers,
                                    CorrespondingId = x.SortOrder,
                                    Type = x.Type,
                                    AppliedModifiers = x
                                        .AppliedModifiers.Select(y => new ModifierDescription()
                                        {
                                            Value = y.Value,
                                            Type = y.Type,
                                            Message = y.Message,
                                            Name = y.Name,
                                        })
                                        .ToList(),
                                })
                                .ToList(),
                        }
                    );
                }
            )
            .WithSummary("Returns all basic proficiencies for the given character")
            .RequireAuthorization();
    }
}
