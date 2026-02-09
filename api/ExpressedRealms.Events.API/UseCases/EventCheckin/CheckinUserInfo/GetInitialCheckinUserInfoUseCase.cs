using ExpressedRealms.Events.API.Repositories.EventCheckin;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.CheckinUserInfo;

internal sealed class GetInitialCheckinUserInfoUseCase(
    IEventCheckinRepository checkinRepository,
    GetInitialCheckinUserInfoModelValidator validator,
    CancellationToken cancellationToken
) : IGetInitialCheckinUserInfoUseCase
{
    public async Task<Result<GetInitialCheckinUserInfoReturnModel>> ExecuteAsync(GetInitialCheckinUserInfoModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);
        
        var playerName = await checkinRepository.GetUserName(model.LookupId);
        var isFirstTimePlayer = await checkinRepository.IsFirstTimePlayer(model.LookupId);

        return Result.Ok(new GetInitialCheckinUserInfoReturnModel()
            {
                Username = playerName,
                IsFirstTimeUser = isFirstTimePlayer
            }
        );
    }
}
