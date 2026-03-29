using ExpressedRealms.Events.API.Repositories.EventCheckin;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.UpdateCrbEmailNotification;

internal sealed class UpdateCrbEmailNotificationUseCase(
    IEventCheckinRepository checkinRepository,
    UpdateCrbEmailNotificationModelValidator validator,
    CancellationToken cancellationToken
) : IUpdateCrbEmailNotificationUseCase
{
    public async Task<Result> ExecuteAsync(UpdateCrbEmailNotificationModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var player = await checkinRepository.GetCurrentPlayerForEditingAsync();
        
        player.SendPickupCrbEmail = model.EnableEmailNotification;

        await checkinRepository.EditAsync(player);

        return Result.Ok();
    }
}
