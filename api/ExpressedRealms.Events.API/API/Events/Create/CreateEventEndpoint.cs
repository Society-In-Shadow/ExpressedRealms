using ExpressedRealms.Events.API.UseCases.Events.Create;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Events.API.API.Events.Create;

public static class CreateEventEndpoint
{
    public static async Task<
        Results<Ok<int>, NotFound, ValidationProblem, BadRequest<string>>
    > ExecuteAsync(
        [FromBody] CreateEventRequest request,
        [FromServices] ICreateEventUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(
            new CreateEventModel()
            {
                Name = request.Name,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Location = request.Location,
                WebsiteName = request.WebsiteName,
                WebsiteUrl = request.WebsiteUrl,
                AdditionalNotes = request.AdditionalNotes,
                ConExperience = request.ConExperience
            }
        );

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        if (results.HasInsufficientXP(out var insufficientXp))
            return insufficientXp;
        if (results.HasNotFound(out var notFound))
            return notFound;

        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok(results.Value);
    }
}
