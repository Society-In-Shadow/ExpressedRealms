namespace ExpressedRealms.Events.API.UseCases.EventCheckin.GetGoCheckinInfo;

public class GetGoCheckinInfoReturnModel
{
    public required string Username { get; set; }
    public bool IsFirstTimeUser { get; set; }
    public bool AlreadyCheckedIn { get; set; }
}
