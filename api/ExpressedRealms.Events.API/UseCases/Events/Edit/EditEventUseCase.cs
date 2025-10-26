using ExpressedRealms.Events.API.Repositories.Events;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.Events.Edit;

internal sealed class EditEventUseCase(
    IEventRepository eventRepository,
    EditEventModelValidator validator,
    CancellationToken cancellationToken
) : IEditEventUseCase
{
    public async Task<Result> ExecuteAsync(EditEventModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var currentEvent = await eventRepository.GetEventAsync(model.Id);

        currentEvent.Name = model.Name;
        currentEvent.StartDate = model.StartDate;
        currentEvent.EndDate = model.EndDate;
        currentEvent.Location = model.Location;
        currentEvent.WebsiteName = model.WebsiteName;
        currentEvent.WebsiteUrl = model.WebsiteUrl;
        currentEvent.AdditionalNotes = model.AdditionalNotes;
        currentEvent.ConExperience = model.ConExperience;
        currentEvent.TimeZoneId = model.TimeZoneId;

        await eventRepository.EditAsync(currentEvent);

        return Result.Ok();
    }
}
