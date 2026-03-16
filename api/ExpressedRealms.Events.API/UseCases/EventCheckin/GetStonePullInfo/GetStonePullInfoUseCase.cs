using ExpressedRealms.DB.Models.Checkins.CheckinStageSetup;
using ExpressedRealms.Events.API.Repositories.EventCheckin;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.GetStonePullInfo;

internal sealed class GetStonePullInfoUseCase(
    IEventCheckinRepository checkinRepository,
    GetStonePullInfoModelValidator validator,
    CancellationToken cancellationToken
) : IGetStonePullInfoUseCase
{
    public async Task<Result<GetStonePullInfoReturnModel>> ExecuteAsync(GetStonePullInfoModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var eventId = await checkinRepository.GetActiveEventId();
        var playerId = await checkinRepository.GetPlayerId(model.LookupId);
        var checkin = await checkinRepository.GetCheckinAsync(eventId!.Value, playerId);

        var isFirstTimePlayer = await checkinRepository.IsFirstTimePlayer(model.LookupId);
        var assignedXp = await checkinRepository.GetAssignedXp(playerId, eventId!.Value);

        var hasCompletedStep = await checkinRepository.GetStageStatus(
            checkin.Id,
            CheckinStageEnum.AssignedXpCheck
        );

        return Result.Ok(
            new GetStonePullInfoReturnModel()
            {
                HasCompletedStep = hasCompletedStep,
                IsFirstTimeUser = isFirstTimePlayer,
                AssignedXp = assignedXp is null
                    ? null
                    : new AssignedXpType()
                    {
                        TypeId = assignedXp.TypeId,
                        Amount = assignedXp.Amount,
                    },
            }
        );
    }
}
