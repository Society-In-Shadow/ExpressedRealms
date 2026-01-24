using ExpressedRealms.Admin.UseCases.UpdateCharacterXp;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Admin.API.AdminCharacterList.UpdateCharacterXp;

public static class UpdateCharacterXpEndpoint
{
    public static async Task<Results<Ok, ValidationProblem, NotFound>> Execute(
        int characterId,
        [FromBody] UpdateCharacterXpRequest request,
        [FromServices] IUpdateCharacterXpUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(
            new UpdateCharacterXpModel() { Id = characterId, PlayerNumber = request.PlayerNumber }
        );

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        if (results.HasNotFound(out var notFound))
            return notFound;
        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok();
    }
}
