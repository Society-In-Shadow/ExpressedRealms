namespace ExpressedRealms.Events.API.API.EventCheckin.ConfirmUserCheckinInfo;

public class GetGoCheckinInfoResponse
{
    public required string PlayerName { get; set; }
    public bool IsFirstTimeUser { get; set; }
    public int CheckinId { get; set; }
    public int PlayerNumber { get; set; }
    public int? AssignedXp { get; set; }
    public PrimaryCharacterInfo? PrimaryCharacterInfo { get; set; }

    public List<QuestionResponse> Questions { get; set; } = new();
}
