using ExpressedRealms.Powers.UseCases.CharacterPower.Create;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Powers.API.CharacterPowerEndpoints.Create;

public static class CreatePowerMappingEndpoint
{
    public static async Task<
        Results<Ok<int>, NotFound, ValidationProblem, BadRequest<string>>
    > CreateMapping(
        int characterId,
        [FromBody] CreatePowerMappingRequest request,
        [FromServices] IAddPowerToCharacterUseCase createKnowledgeUseCase
    )
    {
        var results = await createKnowledgeUseCase.ExecuteAsync(
            new ()
            {
                CharacterId = characterId,
                PowerId = request.PowerId,
                Notes = request.Notes,
            }
        );

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        if (results.HasInsufficientXP(out var insufficientXp))
            return insufficientXp;
        if (results.HasNotFound(out var notFound))
            return notFound;

        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok(results.Value);
    }
}
