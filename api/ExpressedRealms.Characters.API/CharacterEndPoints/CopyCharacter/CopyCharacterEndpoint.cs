using ExpressedRealms.Characters.UseCases.Characters.CopyCharacter;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Characters.API.CharacterEndPoints.CopyCharacter;

internal static class CopyCharacterEndpoint
{
    internal static async Task<Results<NotFound, Ok<int>, ValidationProblem>> ExecuteAsync(
        int characterId,
        [FromBody] CopyCharacterRequest dto,
        [FromServices] ICopyCharacterUseCase repository
    )
    {
        var results = await repository.ExecuteAsync(
            new CopyCharacterModel()
            {
                Id = characterId,
                CharacterName = dto.CharacterName,
            }
        );

        if (results.HasNotFound(out var notFound))
            return notFound;
        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok(results.Value);
    }
}
