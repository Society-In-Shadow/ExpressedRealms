using ExpressedRealms.Events.API.UseCases.Events.Edit;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Events.API.API.Events.Edit;

public static class EditEventEndpoint
{
    public static async Task<Results<Ok, NotFound, ValidationProblem>> ExecuteAsync(
        int id,
        [FromBody] EditEventRequest request,
        [FromServices] IEditEventUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(
            new EditEventModel()
            {
                Id = id,
                Name = request.Name,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Location = request.Location,
                WebsiteName = request.WebsiteName,
                WebsiteUrl = request.WebsiteUrl,
                AdditionalNotes = request.AdditionalNotes,
                ConExperience = request.ConExperience,
                TimeZoneId = request.TimeZoneId,
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
