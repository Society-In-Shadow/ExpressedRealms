using ExpressedRealms.Events.API.UseCases.EventScheduleItems.Delete;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Events.API.API.EventScheduleItem.Delete;

public static class DeleteEventScheduleItemEndpoint
{
    public static async Task<Results<Ok, ValidationProblem>> ExecuteAsync(
        int eventId,
        int id,
        [FromServices] IDeleteEventScheduleItemUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(
            new DeleteEventScheduleItemModel() { Id = id, EventId = eventId }
        );

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;

        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok();
    }
}
