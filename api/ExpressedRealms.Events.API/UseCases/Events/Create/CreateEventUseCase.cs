using ExpressedRealms.DB.Models.Events.EventSetup;
using ExpressedRealms.Events.API.Repositories.Events;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.Events.Create;

internal sealed class CreateEventUseCase(
    IEventRepository eventRepoository,
    CreateEventModelValidator validator,
    CancellationToken cancellationToken
) : ICreateEventUseCase
{
    public async Task<Result<int>> ExecuteAsync(CreateEventModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var eventId = await eventRepoository.CreateEventAsync(
            new Event()
            {
                Name = model.Name,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Location = model.Location,
                WebsiteName = model.WebsiteName,
                WebsiteUrl = model.WebsiteUrl,
                AdditionalNotes = model.AdditionalNotes,
                ConExperience = model.ConExperience
            }
        );
        
        var fridayDate = DateOnly.FromDateTime(GetNextDayOfWeek(model.StartDate, DayOfWeek.Friday).Date);
        var saturdayDate = DateOnly.FromDateTime(GetNextDayOfWeek(model.StartDate, DayOfWeek.Saturday).Date);
        var sundayDate = DateOnly.FromDateTime(GetNextDayOfWeek(model.StartDate, DayOfWeek.Sunday).Date);

        var defaultSchedule = await eventRepoository.GetDefaultScheduleItems();
        
        foreach (var item in defaultSchedule)
        {
            item.EventId = eventId;
            item.Date = item.Date.DayOfWeek switch
            {
                DayOfWeek.Friday => fridayDate,
                DayOfWeek.Saturday => saturdayDate,
                DayOfWeek.Sunday => sundayDate,
                _ => throw new ArgumentOutOfRangeException(item.Date.DayOfWeek.ToString())
            };
        }
        
        await eventRepoository.BulkAddEventScheduleItems(defaultSchedule);
        
        return Result.Ok(eventId);
    }

    private static DateTimeOffset GetNextDayOfWeek(DateTimeOffset currentDate, DayOfWeek dayOfWeek)
    {
        var daysTillDayOfWeek = ((int)dayOfWeek - (int)currentDate.DayOfWeek + 7) % 7;
        return currentDate.AddDays(daysTillDayOfWeek);
    }
}
