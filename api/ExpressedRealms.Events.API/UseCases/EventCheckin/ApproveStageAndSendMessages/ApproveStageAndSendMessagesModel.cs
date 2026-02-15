namespace ExpressedRealms.Events.API.UseCases.EventCheckin.ApproveStageAndSendMessages;

public class ApproveStageAndSendMessageModel
{
    public int StageId { get; set; }
    public required string LookupId { get; set; }
}
