using ExpressedRealms.Events.API.UseCases.EventCheckin.GetGoCheckinInfo;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Events.API.API.EventCheckin.GetGoCheckinInfo;

public static class GetGoCheckinInfoEndpoint
{
    public static async Task<Ok<GetGoCheckinInfoResponse>> ExecuteAsync(
        string lookupId,
        [FromServices] IGetGoCheckinInfoUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(
            new GetGoCheckinInfoModel() { LookupId = Uri.UnescapeDataString(lookupId) }
        );

        if (results.HasNotFound(out _) || results.HasValidationError(out _))
        {
            return TypedResults.Ok(new GetGoCheckinInfoResponse() { WasFound = false });
        }

        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok(
            new GetGoCheckinInfoResponse()
            {
                WasFound = true,
                UserName = results.Value.Username,
            }
        );
    }
}
