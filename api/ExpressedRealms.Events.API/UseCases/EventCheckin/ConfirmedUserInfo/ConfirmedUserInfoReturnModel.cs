using ExpressedRealms.Events.API.Repositories.EventCheckin.Dtos;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.ConfirmedUserInfo;

public class ConfirmedUserInfoReturnModel
{
    public int PlayerNumber { get; set; }
    public PrimaryCharacterInfo? PrimaryCharacterInfo { get; set; }
    public BasicInfo? CurrentStage { get; set; }
}
