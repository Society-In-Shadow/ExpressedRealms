using ExpressedRealms.Events.API.UseCases.EventCheckin.GetGoCheckinInfo;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Events.API.API.EventCheckin.GetGoCheckinInfo;

public static class GetGoCheckinInfoEndpoint
{
    public static async Task<
        Results<Ok<GetGoCheckinInfoResponse>, ValidationProblem, NotFound>
    > ExecuteAsync(string lookupId, [FromServices] IGetGoCheckinInfoUseCase useCase)
    {
        var results = await useCase.ExecuteAsync(
            new GetGoCheckinInfoModel() { LookupId = lookupId }
        );

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        if (results.HasNotFound(out var notFound))
            return notFound;

        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok(
            new GetGoCheckinInfoResponse()
            {
                UserName = results.Value.Username,
                IsFirstTimeUser = results.Value.IsFirstTimeUser,
            }
        );
    }
}
