using ExpressedRealms.Characters.UseCases.Contacts.Delete;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Characters.API.ContactEndpoints.Delete;

public static class DeleteEndpoint
{
    public static async Task<Results<NoContent, NotFound, ValidationProblem>> ExecuteAsync(
        int characterId,
        int contactId,
        [FromServices] IDeleteContactUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(
            new DeleteContactModel() { Id = contactId, CharacterId = characterId }
        );

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        if (results.HasNotFound(out var notFound))
            return notFound;

        results.ThrowIfErrorNotHandled();

        return TypedResults.NoContent();
    }
}
