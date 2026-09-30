using ExpressedRealms.DB.Models.Checkins.CheckinStageMappingSetup;
using ExpressedRealms.DB.Models.Checkins.CheckinStageSetup;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.ApproveStageAndSendMessages;

public static class CheckinWorkflows
{
    public static readonly List<CheckinStageEnum> PreCheckinSequence =
    [
        CheckinStageEnum.PlayerEarlyCheckin,
        CheckinStageEnum.GoApproval,
        CheckinStageEnum.CrbPrinted,
        CheckinStageEnum.CrbAssembled,
        CheckinStageEnum.CrbPickedUp,
    ];

    public static readonly List<CheckinStageEnum> SHQCheckinSequence =
    [
        CheckinStageEnum.AgeCheckApproval,
        CheckinStageEnum.EventQuestionsCheck,
        CheckinStageEnum.CharacterStorageQuestion,
        CheckinStageEnum.AssignedXpCheck
    ];
    
    public static readonly List<CheckinStageEnum> InitialCheckinSequence =
    [
        CheckinStageEnum.AgeCheckApproval,
        CheckinStageEnum.EventQuestionsCheck,
        CheckinStageEnum.CharacterStorageQuestion,
        CheckinStageEnum.AssignedXpCheck,
        CheckinStageEnum.GoApproval,
        CheckinStageEnum.CrbPrinted,
        CheckinStageEnum.CrbAssembled,
        CheckinStageEnum.CrbPickedUp,
        CheckinStageEnum.Day2Checkin,
        CheckinStageEnum.Day3Checkin,
        CheckinStageEnum.FinalStage,
    ];
    
    public static List<CheckinStageMapping> FilterActiveApprovedStages(List<CheckinStageMapping> mappings)
    {
        var activeList = new List<CheckinStageMapping>();
        var latestReapprovedStage = mappings
            .Where(x => x.CheckinStageId == CheckinStageEnum.PlayerNeedsReapproval.Value)
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefault();

        if (latestReapprovedStage is not null)
        {
            activeList.AddRange(
                mappings.Where(x => SHQCheckinSequence.Contains(CheckinStageEnum.FromValue(x.CheckinStageId)))
            );
            activeList.AddRange(
                mappings
                    .Where(x => x.CreatedAt >= latestReapprovedStage.CreatedAt)
                    .OrderByDescending(x => x.CreatedAt)
                    .ToList()
            );
        }
        else
        {
            activeList.AddRange(mappings);
        }

        return activeList.ToList();
    }
}
