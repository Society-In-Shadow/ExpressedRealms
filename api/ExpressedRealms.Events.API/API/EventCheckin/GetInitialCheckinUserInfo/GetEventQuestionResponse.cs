namespace ExpressedRealms.Events.API.API.EventCheckin.GetInitialCheckinUserInfo;

public class GetInitialCheckinUserInfoResponse
{
    public required string UserName { get; set; }
    public bool IsFirstTimeUser { get; set; }
}
