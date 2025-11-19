using ExpressedRealms.Events.API.UseCases.Events.Get;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Events.API.API.Events.GetSummary;

public static class GetSummaryEventsEndpoint
{
    public static async Task<Ok<List<ListItem>>> ExecuteAsync(
        [FromServices] IGetEventsUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync();

        return TypedResults.Ok(
            results.Value.Events.Select(x => new ListItem() { Id = x.Id, Name = x.Name }).ToList()
        );
    }
}
