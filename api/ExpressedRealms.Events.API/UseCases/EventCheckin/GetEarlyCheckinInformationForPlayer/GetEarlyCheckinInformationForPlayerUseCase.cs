using ExpressedRealms.DB.Models.Checkins.CheckinStageSetup;
using ExpressedRealms.Events.API.Repositories.EventCheckin;
using ExpressedRealms.Events.API.Repositories.Events;
using ExpressedRealms.Events.API.UseCases.EventCheckin.ApprovePreApprovalStageAndSendMessages;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.GetEarlyCheckinInformationForPlayer;

internal sealed class GetEarlyCheckinInformationForPlayerUseCase(
    IEventCheckinRepository checkinRepository,
    IEventRepository eventRepository
) : IGetEarlyCheckinInformationForPlayerUseCase
{
    public async Task<Result<GetEarlyCheckinInformationForPlayerReturnModel>> ExecuteAsync()
    {
        var targetEvent = await checkinRepository.GetExclusivePreCheckinEventId();
        
        var playerId = await checkinRepository.GetCurrentPlayerId();
        var hasPaidForCharacterStorage = await checkinRepository.PlayerHasCharacterStorage(playerId);

        if (targetEvent is null || !hasPaidForCharacterStorage)
            return new GetEarlyCheckinInformationForPlayerReturnModel()
            {
                ShowBanner = false
            };
        
        var targetEventDate = await eventRepository.GetFirstScheduledDayForEvent(targetEvent.Value);
        var primaryCharacter = await checkinRepository.GetPrimaryCharacterInformation(playerId);
        var checkin = await checkinRepository.GetCheckinAsync(targetEvent.Value, playerId);
        
        var approvedStages = await checkinRepository.GetActiveApprovedStages(checkin!.Id);
        var activeList = approvedStages.Select(x => CheckinStageEnum.FromValue(x.CheckinStageId)).ToList();
        var currentStageIndex = ApprovePreApprovalStageAndSendMessageUseCase.PreCheckinSequence.FindLastIndex(activeList.Contains);
        var currentStage = ApprovePreApprovalStageAndSendMessageUseCase.PreCheckinSequence[Math.Min(currentStageIndex + 1, ApprovePreApprovalStageAndSendMessageUseCase.PreCheckinSequence.Count)];
        
        return Result.Ok(new GetEarlyCheckinInformationForPlayerReturnModel()
        {
            ShowBanner = true,
            Event = new EventInfo()
            {
                EventId = targetEventDate.EventId,
                EventName = targetEventDate.EventName,
                StartDate = targetEventDate.StartDate.AddDays(-1)
            },
            Character = primaryCharacter is null ? null : new KeyValuePair<int, string>(primaryCharacter.CharacterId, primaryCharacter.CharacterName),
            NextStage = new KeyValuePair<int, string>(currentStage.Value, currentStage.Name)
        });
    }
}
