using ExpressedRealms.Events.API.Repositories.Events;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.Events.GetEvent;

internal sealed class GetEventUseCase(
    IEventRepository eventRepository,
    GetEventModelValidator validator,
    CancellationToken cancellationToken
) : IGetEventUseCase
{
    public async Task<Result<GetEventReturnModel>> ExecuteAsync(GetEventModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var currentEvent = await eventRepository.GetEventAsync(model.Id);

        return Result.Ok(
            new GetEventReturnModel()
            {
                Id = currentEvent.Id,
                Name = currentEvent.Name,
                StartDate = currentEvent.StartDate,
                EndDate = currentEvent.EndDate,
                Location = currentEvent.Location,
                WebsiteName = currentEvent.WebsiteName,
                WebsiteUrl = currentEvent.WebsiteUrl,
                AdditionalNotes = currentEvent.AdditionalNotes,
                ConExperience = currentEvent.ConExperience,
                TimeZoneId = currentEvent.TimeZoneId,
                IsPublished = currentEvent.IsPublished,
                CollectAttendeeInformation = currentEvent.CollectAttendeeInformation,
            }
        );
    }
}
