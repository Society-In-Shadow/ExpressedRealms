using ExpressedRealms.Events.API.Repositories.EventCheckin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Events.API.API.EventCheckin.GetBasicCheckDetails;

public static class GetEventCheckinShowStatusEndpoint
{
    public static async Task<Results<Ok<bool>, ValidationProblem, NotFound>> ExecuteAsync(
        [FromServices] IEventCheckinRepository useCase
    )
    {
        var results = await useCase.GetActiveEventId();
        return TypedResults.Ok(results is not null);
    }
}
