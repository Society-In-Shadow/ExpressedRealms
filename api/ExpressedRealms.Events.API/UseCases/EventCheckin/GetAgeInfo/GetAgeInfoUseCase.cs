using ExpressedRealms.DB.UserProfile.PlayerDBModels.PlayerAgeGroupSetup;
using ExpressedRealms.Events.API.Repositories.EventCheckin;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.GetAgeInfo;

internal sealed class GetAgeInfoUseCase(
    IEventCheckinRepository checkinRepository,
    GetAgeInfoModelValidator validator,
    CancellationToken cancellationToken
) : IGetAgeInfoUseCase
{
    public async Task<Result<GetAgeInfoReturnModel>> ExecuteAsync(GetAgeInfoModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var player = await checkinRepository.GetPlayerAsync(model.LookupId);
        var currentEventStartDate = await checkinRepository.GetActiveEventStartDate();
        
        return Result.Ok(
            new GetAgeInfoReturnModel()
            {
                AgeGroupId = player.AgeGroupId,
                HasBeenVerified = !player.LastAgeGroupCheck.HasValue ? false : 
                    DateOnly.FromDateTime(player.LastAgeGroupCheck.Value.DateTime) >= currentEventStartDate 
                    || player.AgeGroupId == PlayerAgeGroupEnum.Adult,
            }
        );
    }
}
