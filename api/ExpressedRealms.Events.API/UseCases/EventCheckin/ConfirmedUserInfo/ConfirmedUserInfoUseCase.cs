using ExpressedRealms.DB.Models.Checkins.CheckinSetup;
using ExpressedRealms.DB.Models.Checkins.CheckinStageSetup;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.PlayerAgeGroupSetup;
using ExpressedRealms.Events.API.Repositories.EventCheckin;
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

        var isFirstTimePlayer = await checkinRepository.IsFirstTimePlayer(model.LookupId);
        var playerNumber = checkinRepository.GetPlayerNumber(model.LookupId);

        var primaryCharacterInformation = await checkinRepository.GetPrimaryCharacterInformation(
            player.Id
        );
        var assignedXp = await checkinRepository.GetAssignedXp(player.Id, eventId!.Value);

        PrimaryCharacterInfo? characterInfo = null;
        if (primaryCharacterInformation is not null)
        {
            characterInfo = new PrimaryCharacterInfo()
            {
                CharacterId = primaryCharacterInformation.CharacterId,
                CharacterName = primaryCharacterInformation.CharacterName,
            };
        }

        // If user is over 18, automatically approve them, if they haven't been yet
        
        var currentStage = await checkinRepository.GetCurrentStage(checkinId);

        if (player.AgeGroupId == PlayerAgeGroupEnum.Adult && currentStage is null)
        {
            await approveStageAndSendMessageUseCase.ExecuteAsync(new()
            {
                LookupId = model.LookupId,
                StageId = CheckinStageEnum.AgeCheckApproval
            });
            
            currentStage = await checkinRepository.GetCurrentStage(checkinId);
        }

        return Result.Ok(
            new ConfirmedUserInfoReturnModel()
            {
                PlayerNumber = playerNumber,
                CurrentStage = currentStage,
                PrimaryCharacterInfo = characterInfo, // Needed for Go Verification - Just need to return character id
                IsFirstTimeUser = isFirstTimePlayer, // this is the stone pull step only
                AssignedXp = assignedXp is null // Needed for Stone Pull
                    ? null
                    : new AssignedXpType()
                    {
                        TypeId = assignedXp.TypeId,
                        Amount = assignedXp.Amount,
                    },
            }
        );
    }

    private async Task<int> GetCheckinId(int? eventId, Guid playerId)
    {
        int? checkinId = null;

        var checkin = await checkinRepository.GetCheckinAsync(eventId!.Value, playerId);
        checkinId = checkin?.Id;
        if (checkin is null)
        {
            checkinId = await checkinRepository.CreateCheckinAsync(
                new Checkin() { PlayerId = playerId, EventId = eventId!.Value }
            );
        }

        return checkinId!.Value;
    }
}
