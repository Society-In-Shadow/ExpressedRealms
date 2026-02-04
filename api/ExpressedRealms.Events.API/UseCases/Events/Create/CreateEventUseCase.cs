using ExpressedRealms.DB.Models.Events.EventSetup;
using ExpressedRealms.Events.API.Repositories.Events;
using ExpressedRealms.Events.API.UseCases.EventQuestions.PopulateDefaults;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.Events.Create;

internal sealed class CreateEventUseCase(
    IEventRepository eventRepository,
    IPopulateDefaultQuestionsUseCase populateDefaultQuestionsUseCase,
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

        var eventId = await eventRepository.CreateEventAsync(
            new Event()
            {
                Name = model.Name,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Location = model.Location,
                WebsiteName = model.WebsiteName,
                WebsiteUrl = model.WebsiteUrl,
                AdditionalNotes = model.AdditionalNotes,
                ConExperience = model.ConExperience,
                TimeZoneId = model.TimeZoneId,
            }
        );

        var fridayDate = GetNextDayOfWeek(model.StartDate, DayOfWeek.Friday);
        var saturdayDate = GetNextDayOfWeek(model.StartDate, DayOfWeek.Saturday);
        var sundayDate = GetNextDayOfWeek(model.StartDate, DayOfWeek.Sunday);

        var defaultSchedule = await eventRepository.GetDefaultScheduleItems();

        foreach (var item in defaultSchedule)
        {
            item.Id = 0;
            item.EventId = eventId;
            item.Date = item.Date.DayOfWeek switch
            {
                DayOfWeek.Friday => fridayDate,
                DayOfWeek.Saturday => saturdayDate,
                DayOfWeek.Sunday => sundayDate,
                _ => throw new ArgumentOutOfRangeException(item.Date.DayOfWeek.ToString()),
            };
        }

        await eventRepository.BulkAddEventScheduleItems(defaultSchedule);
        await populateDefaultQuestionsUseCase.ExecuteAsync(new PopulateDefaultQuestionsModel() { EventId = eventId });

        return Result.Ok(eventId);
    }

    private static DateOnly GetNextDayOfWeek(DateOnly currentDate, DayOfWeek dayOfWeek)
    {
        var daysTillDayOfWeek = ((int)dayOfWeek - (int)currentDate.DayOfWeek + 7) % 7;
        return currentDate.AddDays(daysTillDayOfWeek);
    }
}
