using ExpressedRealms.Events.API.UseCases.EventCheckin.GetCheckinInfo;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Events.API.API.EventCheckin.GetCheckDetails;

public static class GetEventCheckinInfoEndpoint
{
    public static async Task<
        Results<Ok<GetEventQuestionResponse>, ValidationProblem, NotFound>
    > ExecuteAsync([FromServices] IGetEventCheckinInfoUseCase useCase)
    {
        var results = await useCase.ExecuteAsync();

        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok(
            new GetEventQuestionResponse()
            {
                LookupId = results.Value.LookupId,
                CheckinStageId = results.Value.CheckinStageId,
                EventId = results.Value.EventId
            }
        );
    }
}
