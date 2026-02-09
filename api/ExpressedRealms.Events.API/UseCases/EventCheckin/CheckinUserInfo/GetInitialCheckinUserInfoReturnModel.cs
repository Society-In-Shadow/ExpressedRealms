namespace ExpressedRealms.Events.API.UseCases.EventCheckin.CheckinUserInfo;

public class GetInitialCheckinUserInfoReturnModel
{
    public required string Username { get; set; }
    public bool IsFirstTimeUser { get; set; }
}
