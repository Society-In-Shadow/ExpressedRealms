using ExpressedRealms.Characters.UseCases.AssignedXp.Edit;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Characters.API.AssignedXp.Edit;

public static class EditEndpoint
{
    public static async Task<Results<Ok, NotFound, ValidationProblem>> ExecuteAsync(
        int characterId,
        int mappingId,
        [FromBody] EditRequest request,
        [FromServices] IEditAssignedXpMappingUseCase createKnowledgeUseCase
    )
    {
        var results = await createKnowledgeUseCase.ExecuteAsync(
            new EditAssignedXpMappingModel()
            {
                EventId = request.EventId,
                AssignedXpTypeId = request.AssignedXpTypeId,
                Reason = request.Reason,
                Amount = request.Amount,
                Id = mappingId,
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
