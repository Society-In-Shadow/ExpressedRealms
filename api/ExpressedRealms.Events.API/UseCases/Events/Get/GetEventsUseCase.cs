using ExpressedRealms.Events.API.Repositories.Events;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.Events.Get;

internal sealed class GetEventsUseCase(
    IEventRepository eventRepository
) : IGetEventUseCase
{
    public async Task<Result<EventBaseReturnModel>> ExecuteAsync()
    {
        var events = await eventRepository.GetEventsAsync();

        return Result.Ok(new EventBaseReturnModel()
        {
            Events = events.Select(x => new EventModel()
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
                TimeZoneId = x.TimeZoneId
            }).ToList()
        });
    }
}
