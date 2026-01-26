using ExpressedRealms.DB.Shared;
using ExpressedRealms.Knowledges.UseCases.Knowledges.GetKnowledgeSummary;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Knowledges.API.GetKnowledgeSummary;

public static class GetKnowledgeSummaryEndpoint
{
    public static async Task<
        Results<Ok<List<GenericListDto<int>>>, ValidationProblem, NotFound>
    > ExecuteAsync(IGetKnowledgeSummaryUseCase useCase)
    {
        var results = await useCase.ExecuteAsync();

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        if (results.HasNotFound(out var notFound))
            return notFound;
        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok(results.Value);
    }
}
