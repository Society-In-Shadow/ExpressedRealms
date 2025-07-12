using ExpressedRealms.Knowledges.UseCases.Knowledges.DeleteKnowledge;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Knowledges.API.DeleteKnowledge;

public static class DeleteKnowledgeEndpoint
{
    public static async Task<Results<Ok, NotFound, ValidationProblem>> DeleteKnowledge(
        int id,
        [FromServices] IDeleteKnowledgeUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(new DeleteKnowledgeModel() { Id = id });

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        if (results.HasNotFound(out var notFound))
            return notFound;

        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok();
    }
}
