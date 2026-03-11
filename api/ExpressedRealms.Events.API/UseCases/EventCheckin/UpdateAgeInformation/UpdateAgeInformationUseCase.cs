using ExpressedRealms.DB.UserProfile.PlayerDBModels.PlayerAgeGroupSetup;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;
using ExpressedRealms.Events.API.Repositories.EventCheckin;
using ExpressedRealms.UseCases.Shared;
using FluentResults;
using Microsoft.AspNetCore.Identity;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.UpdateAgeInformation;

internal sealed class UpdateAgeInformationUseCase(
    IEventCheckinRepository checkinRepository,
    UserManager<User> userManager,
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

        if (model.AgeGroupId == PlayerAgeGroupEnum.Child.Value)
        {
            // Violating Terms of Service, lock them out
            var user = await userManager.FindByIdAsync(player.UserId);
            await userManager.UpdateSecurityStampAsync(user!);
            await userManager.SetLockoutEndDateAsync(user!, DateTimeOffset.MaxValue);
        }

        await checkinRepository.EditAsync(player);

        return Result.Ok();
    }
}
