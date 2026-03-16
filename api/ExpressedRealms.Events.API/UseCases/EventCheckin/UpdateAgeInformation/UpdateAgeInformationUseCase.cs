using ExpressedRealms.DB.Models.Checkins.CheckinStageSetup;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.PlayerAgeGroupSetup;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;
using ExpressedRealms.Events.API.Repositories.EventCheckin;
using ExpressedRealms.Events.API.UseCases.EventCheckin.ApproveStageAndSendMessages;
using ExpressedRealms.UseCases.Shared;
using FluentResults;
using Microsoft.AspNetCore.Identity;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.UpdateAgeInformation;

internal sealed class UpdateAgeInformationUseCase(
    IEventCheckinRepository checkinRepository,
    UserManager<User> userManager,
    TimeProvider timeProvider,
    IApproveStageAndSendMessageUseCase approveStageAndSendMessageUseCase,
    UpdateAgeInformationModelValidator validator,
    CancellationToken cancellationToken
) : IUpdateAgeInformationUseCase
{
    public async Task<Result> ExecuteAsync(UpdateAgeInformationModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var player = await checkinRepository.GetPlayerAsync(model.LookupId);

        player.HasSignedConsentForm = model.HasSignedConsentForm;
        player.AgeGroupId = model.AgeGroupId;
        player.LastAgeGroupCheck = timeProvider.GetUtcNow();

        if (model.AgeGroupId == PlayerAgeGroupEnum.Child.Value)
        {
            // Violating Terms of Service, lock them out
            var user = await userManager.FindByIdAsync(player.UserId);
            await userManager.UpdateSecurityStampAsync(user!);
            await userManager.SetLockoutEndDateAsync(user!, DateTimeOffset.MaxValue);
        }

        await checkinRepository.EditAsync(player);
        
        await approveStageAndSendMessageUseCase.ExecuteAsync(new()
        {
            LookupId = model.LookupId,
            StageId = CheckinStageEnum.AgeCheckApproval
        });

        return Result.Ok();
    }
}
