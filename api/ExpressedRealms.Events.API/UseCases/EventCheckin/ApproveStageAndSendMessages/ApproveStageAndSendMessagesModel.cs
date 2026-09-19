namespace ExpressedRealms.Events.API.UseCases.EventCheckin.ApproveStageAndSendMessages;

public class ApproveStageAndSendMessageModel
{
    public int StageId { get; set; }
    public string? LookupId { get; set; }
    public int? CharacterId { get; set; }
}
