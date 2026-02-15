using ExpressedRealms.Events.API.UseCases.EventCheckin.GetUserCheckinInfo;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Events.API.API.EventCheckin.GetUserCheckinDetails;

public static class GetUserCheckinInfoEndpoint
{
    public static async Task<Ok<GetUserCheckinInfoResponse>> ExecuteAsync(
        [FromServices] IGetUserCheckinInfoUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync();
        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok(
            new GetUserCheckinInfoResponse()
            {
                LookupId = results.Value.LookupId,
                CheckinStage = results.Value.CheckinStage is null
                    ? null
                    : new BasicInfo()
                    {
                        Name = results.Value.CheckinStage.Name,
                        Id = results.Value.CheckinStage.Id,
                    },
                EventId = results.Value.EventId,
            }
        );
    }
}
