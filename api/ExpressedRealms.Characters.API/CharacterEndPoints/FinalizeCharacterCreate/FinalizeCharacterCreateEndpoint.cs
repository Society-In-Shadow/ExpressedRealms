using ExpressedRealms.Characters.UseCases.FinalizeCharacterCreate;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Characters.API.CharacterEndPoints.FinalizeCharacterCreate;

internal static class FinalizeCharacterCreateEndpoint
{
    internal static async Task<
        Results<Ok, NotFound, StatusCodeHttpResult, ValidationProblem>
    > Execute(int id, [FromServices] IFinalizeCharacterCreateUseCase repository)
    {
        var status = await repository.ExecuteAsync(new() { CharacterId = id });

        if (status.HasValidationError(out var validation))
            return validation;
        if (status.HasNotFound(out var notFound))
            return notFound;
        if (status.HasBeenDeletedAlready(out var deletedAlready))
            return deletedAlready;
        
        status.ThrowIfErrorNotHandled();

        return TypedResults.Ok();
    }
}
