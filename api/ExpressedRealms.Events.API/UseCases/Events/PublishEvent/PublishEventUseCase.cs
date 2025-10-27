using ExpressedRealms.Events.API.Repositories.Events;
using ExpressedRealms.Events.API.UseCases.Events.SendEventPublishedMessages;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.Events.PublishEvent;

internal sealed class PublishEventUseCase(
    IEventRepository eventRepository,
    PublishEventModelValidator validator,
    ISendEventPublishedMessagesUseCase publishMessageUseCase,
    CancellationToken cancellationToken
) : IPublishEventUseCase
{
    public async Task<Result> ExecuteAsync(PublishEventModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var currentEvent = await eventRepository.FindEventAsync(model.Id);

        currentEvent!.IsPublished = true;

        await eventRepository.EditAsync(currentEvent);

        await publishMessageUseCase.ExecuteAsync(
            new SendEventPublishedMessagesModel() { Id = model.Id }
        );

        return Result.Ok();
    }
}
