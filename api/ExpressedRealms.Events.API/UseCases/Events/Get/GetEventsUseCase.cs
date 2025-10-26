using ExpressedRealms.Authentication;
using ExpressedRealms.Events.API.Repositories.Events;
using ExpressedRealms.Repositories.Shared.ExternalDependencies;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.Events.Get;

internal sealed class GetEventsUseCase(IEventRepository eventRepository, IUserContext userContext) : IGetEventUseCase
{
    public async Task<Result<EventBaseReturnModel>> ExecuteAsync()
    {
        var hasEventManagementPermission = await userContext.CurrentUserHasPolicy(Policies.ManageEvents);
        
        var events = await eventRepository.GetEventsAsync();
        if (!hasEventManagementPermission)
            events = events.Where(x => x.IsPublished).ToList();

        return Result.Ok(
            new EventBaseReturnModel()
            {
                Events = events
                    .Select(x => new EventModel()
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
