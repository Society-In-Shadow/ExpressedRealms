using ExpressedRealms.DB.Models.Powers.PowerPathSetup;

namespace ExpressedRealms.DB.Models.Powers.PowerPathPowerMappingSetup;

public class PowerPathPowerMapping
{
    public int Id { get; set; }
    public int PowerPathId { get; set; }
    public int PowerId { get; set; }
    public int OrderIndex { get; set; }

    public virtual Power Power { get; set; } = null!;
    public virtual PowerPath PowerPath { get; set; } = null!;
}
