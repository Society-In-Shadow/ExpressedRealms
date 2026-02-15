using ExpressedRealms.Events.API.Repositories.EventCheckin.Dtos;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.ConfirmedUserInfo;

public class ConfirmedUserInfoReturnModel
{
    public required string PlayerName { get; set; }
    public bool IsFirstTimeUser { get; set; }
    public int PlayerNumber { get; set; }
    public int CheckinId { get; set; }
    public List<QuestionResponse> Questions { get; set; } = new List<QuestionResponse>();
    public PrimaryCharacterInfo? PrimaryCharacterInfo { get; set; }
    public AssignedXpType? AssignedXp { get; set; }
    public BasicInfo? CurrentStage { get; set; }
}
