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
                    
                    return TypedResults.Ok(new BaseProficiencyResponse()
                    {
                        Proficiencies = results.Value.Select(x => new ProficienciesDto()
                        {
                            DefensiveName = x.DefensiveName,
                            DefensiveValue = x.DefensiveValue,
                            OffensiveName = x.OffensiveName,
                            OffensiveValue = x.OffensiveValue,
                        }).ToList()
                    });
                }
            )
            .WithSummary("Returns all basic proficiencies for the given character")
            .RequireAuthorization();
    }
}
