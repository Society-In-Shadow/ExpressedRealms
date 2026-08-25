using ExpressedRealms.Characters.UseCases.Characters.ToggleExtraMortisUseCase;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Characters.API.CharacterEndPoints.ToggleExtraMortis;

internal static class ToggleExtraMortisEndpoint
{
    internal static async Task<Results<NotFound, NoContent, ValidationProblem>> ExecuteAsync(
        int characterId,
        [FromBody] ToggleExtraMortisRequest dto,
        [FromServices] IToggleExtraMortisUseCase repository
    )
    {
        var status = await repository.ExecuteAsync(
            new ToggleExtraMortisModel()
            {
                Id = characterId,
                HasExtraMortis = dto.HasExtraMortis
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
