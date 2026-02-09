namespace ExpressedRealms.Events.API.UseCases.EventCheckin.GetCheckinInfo;

public class CheckinInfoReturnModel
{
    public required string LookupId { get; set; }
    public int CheckinStageId { get; set; }
    public int EventId { get; set; }
}
