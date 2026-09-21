namespace ExpressedRealms.Events.API.UseCases.EventCheckin.GetEarlyCheckinInformationForPlayer;

public class GetEarlyCheckinInformationForPlayerReturnModel
{
    public bool ShowBanner { get; set; }
    public KeyValuePair<int, string>? Character { get; set; }
    public EventInfo? Event { get; set; }
    public KeyValuePair<int, string>? NextStage { get; set; }
}
