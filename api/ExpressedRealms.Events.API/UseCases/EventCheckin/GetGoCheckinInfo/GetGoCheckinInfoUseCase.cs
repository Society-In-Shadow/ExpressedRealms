using ExpressedRealms.Events.API.Repositories.EventCheckin;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.GetGoCheckinInfo;

internal sealed class GetGoCheckinInfoUseCase(
    IEventCheckinRepository checkinRepository,
    GetGoCheckinInfoModelValidator validator,
    CancellationToken cancellationToken
) : IGetGoCheckinInfoUseCase
{
    public async Task<Result<GetGoCheckinInfoReturnModel>> ExecuteAsync(GetGoCheckinInfoModel model)
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

        return Result.Ok(
            new GetGoCheckinInfoReturnModel()
            {
                Username = playerName,
                IsFirstTimeUser = isFirstTimePlayer,
            }
        );
    }
}
