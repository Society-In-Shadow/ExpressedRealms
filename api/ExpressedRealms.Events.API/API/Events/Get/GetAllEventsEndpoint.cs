using ExpressedRealms.Events.API.UseCases.Events.Get;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpressedRealms.Events.API.API.Events.Get;

public static class GetAllEventsEndpoint
{
    public static async Task<Ok<GetAllBaseResponse>> ExecuteAsync(
        [FromServices] IGetEventsUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync();

        return TypedResults.Ok(
            new GetAllBaseResponse()
            {
                Events = results
                    .Value.Events.Select(x => new EventModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        StartDate = x.StartDate,
                        EndDate = x.EndDate,
                        Location = x.Location,
                        WebsiteName = x.WebsiteName,
                        WebsiteUrl = x.WebsiteUrl,
                        AdditionalNotes = x.AdditionalNotes,
                        ConExperience = x.ConExperience,
                        TimeZoneId = x.TimeZoneId,
                        IsPublished = x.IsPublished,
                    })
                    .ToList(),
            }
        );
    }
}
