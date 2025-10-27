using ExpressedRealms.Events.API.UseCases.Events.PublishEvent;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Events.API.API.Events.Publish;

public static class PublishEventEndpoint
{
    public static async Task<Results<Ok, NotFound, ValidationProblem>> ExecuteAsync(
        int id,
        [FromServices] IPublishEventUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(
            new PublishEventModel()
            {
                Id = id,
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
