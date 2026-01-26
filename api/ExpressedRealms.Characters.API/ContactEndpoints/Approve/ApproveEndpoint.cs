using ExpressedRealms.Characters.UseCases.Contacts.Approve;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Characters.API.ContactEndpoints.Approve;

public static class ApproveEndpoint
{
    public static async Task<Results<NoContent, NotFound, ValidationProblem>> ExecuteAsync(
        int characterId,
        int contactId,
        [FromBody] ApproveRequest request,
        [FromServices] IApproveContactUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(
            new ApproveContactModel()
            {
                Id = contactId,
                CharacterId = characterId,
                Approved = request.IsApproved,
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
