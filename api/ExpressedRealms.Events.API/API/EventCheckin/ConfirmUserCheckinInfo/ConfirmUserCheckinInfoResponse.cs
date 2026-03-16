using ExpressedRealms.Events.API.API.EventCheckin.GetUserCheckinDetails;

namespace ExpressedRealms.Events.API.API.EventCheckin.ConfirmUserCheckinInfo;

public class GetGoCheckinInfoResponse
{
    public int PlayerNumber { get; set; }
    public PrimaryCharacterInfo? PrimaryCharacterInfo { get; set; }
    public BasicInfo? CurrentStage { get; set; }
}
