using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.Events.API.Repositories.Events;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.Events.Delete;

internal sealed class DeleteEventUseCase(
    IEventRepository eventRepository,
    DeleteEventModelValidator validator,
    CancellationToken cancellationToken
) : IDeleteEventUseCase
{
    public async Task<Result> ExecuteAsync(DeleteEventModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var currentEvent = await eventRepository.GetEventAsync(model.Id);

        currentEvent.SoftDelete();

        await eventRepository.EditAsync(currentEvent);

        return Result.Ok();
    }
}
