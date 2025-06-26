namespace ExpressedRealms.DB.Models.Powers.PowerPrerequisiteSetup;

public class PowerPrerequisite
{
    public int Id { get; set; }
    public int PowerId { get; set; }
    public int RequiredAmount { get; set; }
    public virtual Power Power { get; set; } = null!;
}
