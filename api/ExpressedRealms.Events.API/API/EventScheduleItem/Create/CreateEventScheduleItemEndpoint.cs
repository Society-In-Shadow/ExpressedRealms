using ExpressedRealms.Events.API.UseCases.EventScheduleItems.Create;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Events.API.API.EventScheduleItem.Create;

public static class CreateEventScheduleItemEndpoint
{
    public static async Task<Results<Ok<int>, ValidationProblem>> ExecuteAsync(
        int eventId,
        [FromBody] CreateEventScheduleItemRequest request,
        [FromServices] ICreateEventScheduleItemUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(
            new CreateEventScheduleItemModel()
            {
                EventId = eventId,
                Description = request.Description,
                StartTime = TimeOnly.FromDateTime(request.StartTime),
                EndTime = TimeOnly.FromDateTime(request.EndTime),
                Date = request.Date,
            }
        );

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;

        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok(results.Value);
    }
}
