namespace ExpressedRealms.Powers.Repository.PowerPrerequisites.CreatePrerequisiteUseCase;

public class CreatePrerequisiteModel
{
    public int PowerId { get; set; }
    public int RequiredAmount { get; set; }
    public List<int> PrerequisitePowerIds { get; set; } = new();
}
