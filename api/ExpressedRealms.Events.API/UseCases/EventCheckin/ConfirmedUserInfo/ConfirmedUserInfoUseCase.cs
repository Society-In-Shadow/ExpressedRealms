using ExpressedRealms.DB.Models.Checkins.CheckinSetup;
using ExpressedRealms.DB.Models.Checkins.CheckinStageSetup;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.PlayerAgeGroupSetup;
using ExpressedRealms.Events.API.Repositories.EventCheckin;
using ExpressedRealms.Events.API.Repositories.EventCheckin.Dtos;
using ExpressedRealms.Events.API.UseCases.EventCheckin.ApproveStageAndSendMessages;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.ConfirmedUserInfo;

internal sealed class ConfirmedUserInfoUseCase(
    IEventCheckinRepository checkinRepository,
    ConfirmedUserInfoModelValidator validator,
    IApproveStageAndSendMessageUseCase approveStageAndSendMessageUseCase,
    CancellationToken cancellationToken
) : IConfirmedUserInfoUseCase
{
    public async Task<Result<ConfirmedUserInfoReturnModel>> ExecuteAsync(
        ConfirmedUserInfoModel model
    )
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var eventId = await checkinRepository.GetActiveEventId();
        var player = await checkinRepository.GetPlayerAsync(model.LookupId);
        var checkinId = await GetCheckinId(eventId, player.Id);
        var playerNumber = checkinRepository.GetPlayerNumber(model.LookupId);

        var primaryCharacterInformation = await checkinRepository.GetPrimaryCharacterInformation(
            player.Id
        );

        PrimaryCharacterInfo? characterInfo = null;
        if (primaryCharacterInformation is not null)
        {
            characterInfo = new PrimaryCharacterInfo()
            {
                CharacterId = primaryCharacterInformation.CharacterId,
                CharacterName = primaryCharacterInformation.CharacterName,
            };
        }

        var stageInfo = await GetEarliestIncompleteStage(checkinId);
        // If user is over 18, automatically approve them, if they haven't been yet
        if (
            player.AgeGroupId == PlayerAgeGroupEnum.Adult
            && stageInfo == CheckinStageEnum.AgeCheckApproval
        )
        {
            await approveStageAndSendMessageUseCase.ExecuteAsync(
                new() { LookupId = model.LookupId, StageId = CheckinStageEnum.AgeCheckApproval }
            );

            stageInfo = await GetEarliestIncompleteStage(checkinId);
        }

        var currentEventDay = await checkinRepository.GetCurrentEventDay();

        return Result.Ok(
            new ConfirmedUserInfoReturnModel()
            {
                PlayerNumber = playerNumber,
                CurrentStage = new BasicInfo() { Id = stageInfo.Value, Name = stageInfo.Name },
                PrimaryCharacterInfo = characterInfo, // Needed for Go Verification - Just need to return character id
                CurrentEventDay = currentEventDay, // Needed to determine when to show day 2 / 3 checkin info
            }
        );
    }

    // This needs to happen during character management grab
    private async Task<CheckinStageEnum> GetEarliestIncompleteStage(int checkinId)
    {
        var activeList = await checkinRepository.GetActiveApprovedStages(checkinId);
        var completedStages = activeList
            .Select(x => CheckinStageEnum.FromValue(x.CheckinStageId))
            .ToList();

        var earliestIncomplete = CheckinWorkflows.InitialCheckinSequence.FirstOrDefault(x =>
            !completedStages.Contains(x)
        );

        var latestCompleted = CheckinWorkflows.InitialCheckinSequence.LastOrDefault(x =>
            completedStages.Contains(x)
        );

        return (earliestIncomplete ?? latestCompleted)!;
    }

    private async Task<int> GetCheckinId(int? eventId, Guid playerId)
    {
        int? checkinId = null;

        var checkin = await checkinRepository.GetCheckinAsync(eventId!.Value, playerId);
        checkinId = checkin?.Id;
        if (checkin is null)
        {
            checkinId = await checkinRepository.CreateCheckinAsync(
                new Checkin() { PlayerId = playerId, EventId = eventId.Value }
            );
        }

        return checkinId!.Value;
    }
}
