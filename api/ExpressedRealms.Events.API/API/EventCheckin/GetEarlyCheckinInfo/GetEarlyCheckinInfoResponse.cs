namespace ExpressedRealms.Events.API.API.EventCheckin.GetEarlyCheckinInfo;

public class GetEarlyCheckinInfoResponse
{
    public bool ShowBanner { get; set; }
    public KeyValuePair<int, string>? Character { get; set; }
    public EventInfo? Event { get; set; }
    public KeyValuePair<int, string>? NextStage { get; set; }
}
