using ExpressedRealms.Characters.UseCases.Contacts.Edit;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Characters.API.ContactEndpoints.Edit;

public static class EditEndpoint
{
    public static async Task<Results<NoContent, NotFound, ValidationProblem>> ExecuteAsync(
        int characterId,
        int contactId,
        [FromBody] EditRequest request,
        [FromServices] IEditContactUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(
            new EditContactModel()
            {
                Id = contactId,
                CharacterId = characterId,
                Name = request.Name,
                ContactFrequency = request.ContactFrequency,
                KnowledgeLevel = request.KnowledgeLevel,
                Notes = request.Notes,
            }
        );

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        if (results.HasNotFound(out var notFound))
            return notFound;

        results.ThrowIfErrorNotHandled();

        return TypedResults.NoContent();
    }
}
