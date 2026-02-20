using ExpressedRealms.Characters.UseCases.Characters.RetireCharacter;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Characters.API.CharacterEndPoints.RetireCharacter;

internal static class RetireCharacterEndpoint
{
    internal static async Task<Results<NotFound, NoContent, ValidationProblem>> Execute(
        string lookupId,
        [FromServices] IRetireCharacterUseCase repository
    )
    {
        var status = await repository.ExecuteAsync(
            new RetireCharacterModel()
            {
                LookupId = lookupId
            }
        );

        if (status.HasNotFound(out var notFound))
            return notFound;
        if (status.HasValidationError(out var validationProblem))
            return validationProblem;
        status.ThrowIfErrorNotHandled();

        return TypedResults.NoContent();
    }
}
