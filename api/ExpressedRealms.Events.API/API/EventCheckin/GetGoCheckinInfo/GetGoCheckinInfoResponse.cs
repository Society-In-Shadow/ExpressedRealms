namespace ExpressedRealms.Events.API.API.EventCheckin.GetGoCheckinInfo;

public class GetGoCheckinInfoResponse
{
    public string? UserName { get; set; }
    public bool IsFirstTimeUser { get; set; }
    public bool AlreadyCheckedIn { get; set; }
    public bool WasFound { get; set; }
}
