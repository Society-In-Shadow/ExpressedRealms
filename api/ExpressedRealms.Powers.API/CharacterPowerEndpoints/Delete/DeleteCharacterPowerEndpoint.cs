using ExpressedRealms.Powers.UseCases.CharacterPower.Delete;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Powers.API.CharacterPowerEndpoints.Delete;

public static class DeleteCharacterPowerEndpoint
{
    public static async Task<Results<Ok, NotFound, ValidationProblem>> DeleteMapping(
        int characterId,
        int mappingId,
        [FromServices] IDeletePowerFromCharacterUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(new() { MappingId = mappingId });

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        if (results.HasNotFound(out var notFound))
            return notFound;

        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok();
    }
}
