using ExpressedRealms.Events.API.Repositories.EventCheckin.Dtos;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.GetUserCheckinInfo;

public class GetUserCheckinInfoReturnModel
{
    public required string LookupId { get; set; }
    public BasicInfo? CheckinStage { get; set; }
    public required string EventName { get; set; } = null!;
    public bool SendPickupCrbEmail { get; set; }
}
