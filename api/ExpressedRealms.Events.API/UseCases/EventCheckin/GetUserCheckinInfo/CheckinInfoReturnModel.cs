namespace ExpressedRealms.Events.API.UseCases.EventCheckin.GetUserCheckinInfo;

public class GetUserCheckinInfoReturnModel
{
    public required string LookupId { get; set; }
    public int CheckinStageId { get; set; }
    public int EventId { get; set; }
}
