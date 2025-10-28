using ExpressedRealms.Events.API.UseCases.EventScheduleItems.Get;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Events.API.API.EventScheduleItem.Get;

public static class GetAllEventScheduleItemsEndpoint
{
    public static async Task<
        Results<Ok<GetAllBaseResponse>, NotFound, ValidationProblem>
    > ExecuteAsync(int eventId, [FromServices] IGetEventScheduleItemUseCase useCase)
    {
        var results = await useCase.ExecuteAsync(
            new GetEventScheduleItemModel() { EventId = eventId }
        );

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        if (results.HasNotFound(out var notFound))
            return notFound;

        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok(
            new GetAllBaseResponse()
            {
                EventScheduleItems = results
                    .Value.EventScheduleItems.Select(x => new EventScheduleItemModel()
                    {
                        Id = x.Id,
                        Date = x.Date,
                        Description = x.Description,
                        EndTime = x.Date.ToDateTime(x.EndTime),
                        StartTime = x.Date.ToDateTime(x.StartTime),
                        EventId = x.EventId,
                    })
                    .ToList(),
            }
        );
    }
}
