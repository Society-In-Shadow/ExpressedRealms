using ExpressedRealms.Events.API.UseCases.EventCheckin.GetEarlyCheckinInformationForPlayer;
using ExpressedRealms.FeatureFlags;
using ExpressedRealms.FeatureFlags.FeatureClient;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Events.API.API.EventCheckin.GetEarlyCheckinInfo;

public static class GetEarlyCheckinInfoEndpoint
{
    public static async Task<Ok<GetEarlyCheckinInfoResponse>> ExecuteAsync(
        [FromServices] IGetEarlyCheckinInformationForPlayerUseCase useCase,
        [FromServices] IFeatureToggleClient toggleClient
    )
    {
        if (!await toggleClient.HasFeatureFlag(ReleaseFlags.ShowPreCheckinFunctionality))
        {
            return TypedResults.Ok(new GetEarlyCheckinInfoResponse() { ShowBanner = false });
        }

        var results = await useCase.ExecuteAsync();

        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok(
            new GetEarlyCheckinInfoResponse()
            {
                ShowBanner = results.Value.ShowBanner,
                Event = results.Value.Event is null
                    ? null
                    : new EventInfo()
                    {
                        EventId = results.Value.Event.EventId,
                        EventName = results.Value.Event.EventName,
                        StartDate = results.Value.Event.StartDate,
                    },
                Character = results.Value.Character,
                NextStage = results.Value.NextStage,
            }
        );
    }
}
