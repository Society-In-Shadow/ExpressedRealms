namespace ExpressedRealms.Events.API.UseCases.EventCheckin.GetStonePullInfo;

public class GetStonePullInfoReturnModel
{
    public bool IsFirstTimeUser { get; set; }
    public AssignedXpType? AssignedXp { get; set; }
    public bool HasCompletedStep { get; set; }
}
