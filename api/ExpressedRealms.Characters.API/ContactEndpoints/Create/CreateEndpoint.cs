using ExpressedRealms.Characters.UseCases.Contacts.Create;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Characters.API.ContactEndpoints.Create;

public static class CreateEndpoint
{
    public static async Task<Results<Ok<int>, NotFound, ValidationProblem>> ExecuteAsync(
        int characterId,
        [FromBody] CreateRequest request,
        [FromServices] ICreateContactUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(
            new CreateContactModel()
            {
                CharacterId = characterId,
                KnowledgeId = request.KnowledgeId,
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

        return TypedResults.Ok(results.Value);
    }
}
