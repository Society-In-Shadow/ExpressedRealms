using ExpressedRealms.DB.Models.Powers.CharacterPowerMappingSetup;

namespace ExpressedRealms.DB.Models.Powers;

public class PowerLevel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int Xp { get; set; }
    public int TotalXp { get; set; }

    public virtual List<Power> Powers { get; set; } = null!;
    public virtual List<CharacterPowerMapping> CharacterPowerMappings { get; set; } = null!;
}
