using ExpressedRealms.Events.API.UseCases.EventCheckin.GetGoCheckinInfo;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Events.API.API.EventCheckin.GetGoCheckinInfo;

public static class GetGoCheckinInfoEndpoint
{
    public static async Task<Results<Ok<GetGoCheckinInfoResponse>, ValidationProblem>> ExecuteAsync(
        string lookupId,
        [FromServices] IGetGoCheckinInfoUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(
            new GetGoCheckinInfoModel() { LookupId = lookupId }
        );

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        if (results.HasNotFound(out _))
        {
            return TypedResults.Ok(new GetGoCheckinInfoResponse() { WasFound = false });
        }

        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok(
            new GetGoCheckinInfoResponse()
            {
                WasFound = true,
                UserName = results.Value.Username,
                IsFirstTimeUser = results.Value.IsFirstTimeUser,
                AlreadyCheckedIn = results.Value.AlreadyCheckedIn,
            }
        );
    }
}
