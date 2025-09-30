using ExpressedRealms.DB.Models.Blessings.BlessingLevelSetup;
using ExpressedRealms.DB.Models.Expressions.ProgressionPathSetup.ProgressionLevels;
using ExpressedRealms.DB.Models.ModifierSystem.StatGroupMappings;
using ExpressedRealms.DB.Models.Powers;

namespace ExpressedRealms.DB.Models.ModifierSystem.StatModifierGroups;

public class StatModifierGroup
{
    public int Id { get; set; }

    public virtual List<StatGroupMapping> StatGroupMappings { get; set; } = null!;
    public virtual List<Power> Powers { get; set; } = null!;
    public virtual List<BlessingLevel> BlessingLevels { get; set; } = null!;
    public virtual List<ProgressionLevel> ProgressionLevels { get; set; } = null!;
}
