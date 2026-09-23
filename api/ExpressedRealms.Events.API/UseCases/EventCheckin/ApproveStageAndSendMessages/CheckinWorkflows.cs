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
}