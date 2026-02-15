namespace ExpressedRealms.Events.API.API.EventCheckin.GetUserCheckinDetails;

public class GetUserCheckinInfoResponse
{
    public required string LookupId { get; set; }
    public BasicInfo? CheckinStage { get; set; }
    public int EventId { get; set; }
}
