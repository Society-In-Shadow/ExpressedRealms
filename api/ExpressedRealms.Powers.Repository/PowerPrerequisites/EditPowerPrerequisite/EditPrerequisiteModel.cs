namespace ExpressedRealms.Powers.Repository.PowerPrerequisites.EditPowerPrerequisite;

public class EditPrerequisiteModel
{
    public int Id { get; set; }
    public int RequiredAmount { get; set; }
    public List<int> PowerIds { get; set; } = new();
}