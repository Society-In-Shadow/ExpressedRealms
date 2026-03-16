using ExpressedRealms.Events.API.API.EventCheckin.GetUserCheckinDetails;

namespace ExpressedRealms.Events.API.API.EventCheckin.GetStonePullInfo;

public class GetGoCheckinInfoResponse
{
    public bool HasCompletedStep { get; set; }
    public bool IsFirstTimeUser { get; set; }
    public AssignedXpType? AssignedXp { get; set; }
}
