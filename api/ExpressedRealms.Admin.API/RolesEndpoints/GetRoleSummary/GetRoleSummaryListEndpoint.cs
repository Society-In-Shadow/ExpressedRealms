using ExpressedRealms.Admin.UseCases.Roles.GetRoleSummary;
using ExpressedRealms.DB.Shared;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Admin.API.RolesEndpoints.GetRoleSummary;

public static class GetRoleSummaryEndpoint
{
    public static async Task<Results<Ok<List<GenericListDto<int>>>, ValidationProblem, NotFound>> Execute(
        IGetRoleSummaryUseCase useCase
    )
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
