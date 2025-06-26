using ExpressedRealms.DB.Models.Powers.PowerPrerequisiteSetup;

namespace ExpressedRealms.DB.Models.Powers.PowerPrerequisitePowerSetup;

public class PowerPrerequisitePower
{
    public int PrerequisiteId { get; set; }
    public int PowerId { get; set; }
    public virtual Power Power { get; set; } = null!;
    public virtual PowerPrerequisite Prerequisite { get; set; } = null!;
}
