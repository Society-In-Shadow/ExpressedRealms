namespace ExpressedRealms.Events.API.API.EventCheckin.GetCheckDetails;

public class GetEventQuestionResponse
{
    public required string LookupId { get; set; }
    public int CheckinStageId { get; set; }
    public int EventId { get; set; }
}
