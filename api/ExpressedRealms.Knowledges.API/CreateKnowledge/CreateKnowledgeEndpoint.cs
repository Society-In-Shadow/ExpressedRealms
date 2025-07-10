using ExpressedRealms.Knowledges.UseCases.Knowledges.CreateKnowledge;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Knowledges.API.CreateKnowledge;

internal static class CreateKnowledgeEndpoint
{
    public static async Task<Results<Ok<int>, NotFound, ValidationProblem>> CreateKnowledge(CreateKnowledgeRequest request, ICreateKnowledgeUseCase editKnowledgeUseCase)
    {
        var results = await editKnowledgeUseCase.ExecuteAsync(new CreateKnowledgeModel()
        {
            Name = request.Name,
            Description = request.Description,
            KnowledgeTypeId = request.KnowledgeTypeId,
        });

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        if (results.HasNotFound(out var notFound))
            return notFound;

        results.ThrowIfErrorNotHandled();
        
        return TypedResults.Ok(results.Value);
    }
}