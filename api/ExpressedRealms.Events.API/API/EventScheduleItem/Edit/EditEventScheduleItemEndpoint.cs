using ExpressedRealms.Events.API.UseCases.EventScheduleItems.Edit;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Events.API.API.EventScheduleItem.Edit;

public static class EditEventScheduleItemEndpoint
{
    public static async Task<Results<Ok, ValidationProblem>> ExecuteAsync(
        int eventId,
        int id,
        [FromBody] EditEventScheduleItemRequest request,
        [FromServices] IEditEventScheduleItemUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(
            new EditEventScheduleItemModel()
            {
                Id = id,
                EventId = eventId,
                Description = request.Description,
                Date = request.Date,
                StartTime = TimeOnly.FromDateTime(request.StartTime),
                EndTime = TimeOnly.FromDateTime(request.EndTime),
            }
        );

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;

        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok();
    }
}
