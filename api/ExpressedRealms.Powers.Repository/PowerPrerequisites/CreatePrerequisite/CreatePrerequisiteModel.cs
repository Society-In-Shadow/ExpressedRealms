namespace ExpressedRealms.Powers.Repository.PowerPrerequisites.CreatePrerequisite;

public class CreatePrerequisiteModel
{
    public int PowerId { get; set; }
    public int RequiredAmount { get; set; }
    public List<int> PrerequisitePowerIds { get; set; } = new();
}
