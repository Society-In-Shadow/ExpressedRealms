using ExpressedRealms.Events.API.Repositories.Events;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventScheduleItems.Get;

internal sealed class GetEventScheduleItemUseCase(
    IEventRepository eventRepository,
    GetEventScheduleItemModelValidator validator,
    CancellationToken cancellationToken
) : IGetEventScheduleItemUseCase
{
    public async Task<Result<EventScheduleItemBaseReturnModel>> ExecuteAsync(
        GetEventScheduleItemModel model
    )
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var scheduleItems = await eventRepository.GetEventScheduleItems(model.EventId);

        return Result.Ok(
            new EventScheduleItemBaseReturnModel()
            {
                EventScheduleItems = scheduleItems
                    .Select(x => new EventScheduleItemModel()
                    {
                        Id = x.Id,
                        Date = x.Date,
                        Description = x.Description,
                        EndTime = x.EndTime,
                        StartTime = x.StartTime,
                        EventId = x.EventId,
                    })
                    .ToList(),
            }
        );
    }
}
