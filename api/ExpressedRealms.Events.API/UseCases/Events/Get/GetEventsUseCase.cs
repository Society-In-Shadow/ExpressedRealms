using ExpressedRealms.Authentication.PermissionCollection;
using ExpressedRealms.Events.API.Repositories.Events;
using ExpressedRealms.Repositories.Shared.ExternalDependencies;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.Events.Get;

internal sealed class GetEventsUseCase(IEventRepository eventRepository, IUserContext userContext)
    : IGetEventsUseCase
{
    public async Task<Result<EventBaseReturnModel>> ExecuteAsync()
    {
        var hasViewEvents = await userContext.CurrentUserHasPermission(Permissions.Event.View);

        var events = await eventRepository.GetEventsAsync();
        if (!hasViewEvents)
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
