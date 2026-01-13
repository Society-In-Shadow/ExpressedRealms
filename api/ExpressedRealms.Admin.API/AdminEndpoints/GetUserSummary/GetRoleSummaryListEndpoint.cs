using ExpressedRealms.Admin.UseCases.Users.GetUserSummary;
using ExpressedRealms.DB.Shared;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Admin.API.AdminEndpoints.GetUserSummary;

public static class GetUserSummaryEndpoint
{
    public static async Task<
        Results<Ok<List<GenericListDto<string>>>, ValidationProblem, NotFound>
    > Execute(IGetUserSummaryUseCase useCase)
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
