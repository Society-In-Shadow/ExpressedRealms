using ExpressedRealms.Events.API.UseCases.EventCheckin.GetAgeInfo;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Events.API.API.EventCheckin.GetAgeInfo;

public static class GetAgeInfoEndpoint
{
    public static async Task<Results<Ok<GetAgeInfoResponse>, ValidationProblem, NotFound>> ExecuteAsync(
        string lookupId,
        [FromServices] IGetAgeInfoUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(
            new GetAgeInfoModel() { LookupId = Uri.UnescapeDataString(lookupId) }
        );

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        if (results.HasNotFound(out var notFound))
            return notFound;

        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok(
            new GetAgeInfoResponse()
            {
                AgeGroupId = results.Value.AgeGroupId,
                HasBeenVerified = results.Value.HasBeenVerified,
            }
        );
    }
}
