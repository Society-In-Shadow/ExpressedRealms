using ExpressedRealms.DB.Models.ModifierSystem.StatGroupMappings;

namespace ExpressedRealms.DB.Models.ModifierSystem.StatModifiers;

public class StatModifier
{
    public int Id { get; set; }
    public string Name { get; set; }

    public virtual List<StatGroupMapping> StatGroupMappings { get; set; } = null!;
}
