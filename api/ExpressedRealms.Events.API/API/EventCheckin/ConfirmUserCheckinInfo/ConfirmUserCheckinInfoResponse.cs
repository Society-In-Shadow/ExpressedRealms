using ExpressedRealms.Events.API.API.EventCheckin.GetUserCheckinDetails;

namespace ExpressedRealms.Events.API.API.EventCheckin.ConfirmUserCheckinInfo;

public class GetGoCheckinInfoResponse
{
    public required string PlayerName { get; set; }
    public bool IsFirstTimeUser { get; set; }
    public int CheckinId { get; set; }
    public int PlayerNumber { get; set; }
    public AssignedXpType? AssignedXp { get; set; }
    public PrimaryCharacterInfo? PrimaryCharacterInfo { get; set; }

    public List<QuestionResponse> Questions { get; set; } = new();
    public BasicInfo? CurrentStage { get; set; }
}
