using ExpressedRealms.Knowledges.UseCases.Knowledges.EditKnowledge;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Knowledges.API.EditKnowledge;

public static class EditKnowledgeEndpoint
{
    public static async Task<Results<Ok, NotFound, ValidationProblem>> EditKnowledges(
        int id,
        [FromBody] EditKnowledgeRequest request,
        [FromServices] IEditKnowledgeUseCase editKnowledgeUseCase
    )
    {
        var results = await editKnowledgeUseCase.ExecuteAsync(
            new EditKnowledgeModel()
            {
                Id = id,
                Name = request.Name,
                Description = request.Description,
                KnowledgeTypeId = request.KnowledgeTypeId,
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
