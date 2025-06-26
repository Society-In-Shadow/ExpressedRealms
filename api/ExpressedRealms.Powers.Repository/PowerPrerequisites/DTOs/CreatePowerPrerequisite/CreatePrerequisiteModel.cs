namespace ExpressedRealms.Powers.Repository.PowerPrerequisites.DTOs.CreatePowerPrerequisite;

public class CreatePrerequisiteModel
{
    public int Id { get; set; }
    public int RequiredAmount { get; set; }
    public List<int> PowerIds { get; set; } = new();
}