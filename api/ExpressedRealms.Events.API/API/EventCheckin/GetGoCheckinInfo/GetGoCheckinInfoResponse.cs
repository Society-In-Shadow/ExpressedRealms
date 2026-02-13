namespace ExpressedRealms.Events.API.API.EventCheckin.GetGoCheckinInfo;

public class GetGoCheckinInfoResponse
{
    public required string UserName { get; set; }
    public bool IsFirstTimeUser { get; set; }
    public bool AlreadyCheckedIn { get; set; }
}
