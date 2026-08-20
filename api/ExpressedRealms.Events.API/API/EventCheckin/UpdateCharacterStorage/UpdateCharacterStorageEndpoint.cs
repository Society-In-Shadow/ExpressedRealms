using ExpressedRealms.Events.API.UseCases.EventCheckin.UpdateCharacterStorageInfo;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Events.API.API.EventCheckin.UpdateCharacterStorage;

public static class UpdateCharacterStorageInfoEndpoint
{
    public static async Task<Results<Ok, ValidationProblem, NotFound>> ExecuteAsync(
        string lookupId,
        [FromServices] IUpdateCharacterStorageInfoUseCase useCase,
        [FromBody] UpdateCharacterStorageInfoRequest model
    )
    {
        var results = await useCase.ExecuteAsync(
            new ()
            {
                LookupId = Uri.UnescapeDataString(lookupId),
                OptedIn = model.OptedIn,
            }
        );

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        if (results.HasNotFound(out var notFound))
            return notFound;

        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok();
    }
}
