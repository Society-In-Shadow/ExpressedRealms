using ExpressedRealms.Events.API.UseCases.EventCheckin.UpdateCrbEmailNotification;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Events.API.API.EventCheckin.UpdateCrbEmailNotification;

public static class UpdateCrbEmailNotificationEndpoint
{
    public static async Task<Results<Ok, ValidationProblem, NotFound>> ExecuteAsync(
        [FromServices] IUpdateCrbEmailNotificationUseCase useCase,
        [FromBody] UpdateCrbEmailNotificationRequest model
    )
    {
        var results = await useCase.ExecuteAsync(
            new UpdateCrbEmailNotificationModel()
            {
                EnableEmailNotification = model.EnableEmailNotification,
            }
        );

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        if (results.HasNotFound(out var notFound))
            return notFound;

        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok();
    }
}
