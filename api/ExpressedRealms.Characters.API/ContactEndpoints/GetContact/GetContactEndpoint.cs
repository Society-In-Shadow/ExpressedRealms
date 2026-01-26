using ExpressedRealms.Characters.UseCases.Contacts.GetContact;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Characters.API.ContactEndpoints.GetContact;

public static class GetContactEndpoint
{
    public static async Task<
        Results<Ok<ContactResponse>, NotFound, ValidationProblem>
    > ExecuteAsync(int characterId, int contactId, [FromServices] IGetContactUseCase useCase)
    {
        var results = await useCase.ExecuteAsync(
            new GetContactModel() { Id = contactId, CharacterId = characterId }
        );

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        if (results.HasNotFound(out var notFound))
            return notFound;

        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok(
            new ContactResponse()
            {
                KnowledgeLevelId = results.Value.KnowledgeLevelId,
                Name = results.Value.Name,
                KnowledgeId = results.Value.KnowledgeId,
                Id = results.Value.Id,
                IsApproved = results.Value.IsApproved,
                UsesPerWeek = results.Value.UsesPerWeek,
                Notes = results.Value.Notes,
            }
        );
    }
}
