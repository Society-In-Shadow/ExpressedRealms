using ExpressedRealms.DB.Models.Powers.PowerPrerequisitePowerSetup;

namespace ExpressedRealms.DB.Models.Powers.PowerPrerequisiteSetup;

public class PowerPrerequisite
{
    public int Id { get; set; }
    public int PowerId { get; set; }
    public int RequiredAmount { get; set; }
    public int? CloneSourceId { get; set; }
    public Guid? CloneBatchId { get; set; }
    public virtual Power Power { get; set; } = null!;
    public PowerPrerequisite? CloneSource { get; set; }
    public virtual List<PowerPrerequisitePower> PrerequisitePowers { get; set; } = null!;
    public virtual ICollection<PowerPrerequisite> Clones { get; set; } =
        new HashSet<PowerPrerequisite>();
}
