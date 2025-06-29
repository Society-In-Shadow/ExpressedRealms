namespace ExpressedRealms.Powers.Repository.PowerPrerequisites.EditPrerequisite;

public class EditPrerequisiteModel
{
    public int Id { get; set; }
    public int RequiredAmount { get; set; }
    public List<int> PowerIds { get; set; } = new();
}