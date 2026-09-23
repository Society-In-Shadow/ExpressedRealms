using ExpressedRealms.DB.Models.Checkins.CheckinStageMappingSetup;
using ExpressedRealms.DB.Models.Checkins.CheckinStageSetup;
using ExpressedRealms.Events.API.Repositories.EventCheckin;
using ExpressedRealms.Events.API.Repositories.EventCheckin.Dtos;
using ExpressedRealms.Events.API.UseCases.EventCheckin.ApproveStageAndSendMessages;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.GetUserCheckinInfo;

internal sealed class GetUserCheckinInfoUseCase(IEventCheckinRepository checkinRepository)
    : IGetUserCheckinInfoUseCase
{
    public async Task<Result<GetUserCheckinInfoReturnModel>> ExecuteAsync()
    {
        var playerInfo = await checkinRepository.GetPlayerInfoForPlayerCheckinPage();
        if (string.IsNullOrWhiteSpace(playerInfo.EventName))
        {
            return Result.Fail("No Active Event Found");
        }

        BasicInfo? currentStage = null;
        if (playerInfo.CheckinId is not null)
        {
            var stages = await checkinRepository.GetActiveApprovedStages(playerInfo.CheckinId.Value);
            var activeStep = GetActiveStatus(stages);
            currentStage = new BasicInfo()
            {
                Id = activeStep.Value,
                Name = activeStep.Name
            };
        }

        return Result.Ok(
            new GetUserCheckinInfoReturnModel()
            {
                LookupId = playerInfo.LookupId,
                SendPickupCrbEmail = playerInfo.SendPickupCrbEmail,
                CheckinStage = currentStage,
                EventName = playerInfo.EventName,
            }
        );
    }
    
    private static CheckinStageEnum GetActiveStatus(List<CheckinStageMapping> stages)
    {
        var completedStages = stages.Select(x => CheckinStageEnum.FromValue(x.CheckinStageId)).ToList();
        // This will always be the next step needed to be completed
        var earliestIncomplete = CheckinWorkflows.InitialCheckinSequence.First(x =>
            !completedStages.Contains(x)
        );

        var initialCheckinSteps = CheckinWorkflows.InitialCheckinSequence.IndexOf(CheckinStageEnum.AssignedXpCheck);
        var earliestCheckinStep = CheckinWorkflows.InitialCheckinSequence.IndexOf(earliestIncomplete);

        var lastCompletedStep = CheckinWorkflows.InitialCheckinSequence[Math.Max(earliestCheckinStep - 1, 0)];
        if (earliestIncomplete == CheckinStageEnum.CrbPrinted || lastCompletedStep == CheckinStageEnum.CrbPrinted)
            return CheckinStageEnum.GoApproval;

        if (earliestIncomplete == CheckinStageEnum.GoApproval)
            return CheckinStageEnum.AssignedXpCheck;
        
        if (earliestCheckinStep <= initialCheckinSteps)
            return CheckinStageEnum.AwaitingInitialCheckin;
        
        return CheckinWorkflows.InitialCheckinSequence[earliestCheckinStep];
    }
}
