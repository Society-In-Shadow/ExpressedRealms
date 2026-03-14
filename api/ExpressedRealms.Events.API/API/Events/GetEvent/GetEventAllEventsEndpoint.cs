using ExpressedRealms.Events.API.UseCases.Events.GetEvent;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Events.API.API.Events.GetEvent;

public static class GetEventEndpoint
{
    public static async Task<
        Results<Ok<EventModelResponse>, NotFound, ValidationProblem>
    > ExecuteAsync(int id, [FromServices] IGetEventUseCase useCase)
    {
        var results = await useCase.ExecuteAsync(new GetEventModel() { Id = id });

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        if (results.HasNotFound(out var notFound))
            return notFound;

        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok(
            new EventModelResponse()
            {
                Id = results.Value.Id,
                Name = results.Value.Name,
                StartDate = results.Value.StartDate,
                EndDate = results.Value.EndDate,
                Location = results.Value.Location,
                WebsiteName = results.Value.WebsiteName,
                WebsiteUrl = results.Value.WebsiteUrl,
                AdditionalNotes = results.Value.AdditionalNotes,
                ConExperience = results.Value.ConExperience,
                TimeZoneId = results.Value.TimeZoneId,
                IsPublished = results.Value.IsPublished,
                CollectConAttendance = results.Value.CollectAttendeeInformation,
            }
        );
    }
}
