using ExpressedRealms.DB.Models.Expressions.ExpressionSetup;
using ExpressedRealms.DB.Models.ModifierSystem.StatModifierGroups;
using ExpressedRealms.DB.Models.ModifierSystem.StatModifiers;

namespace ExpressedRealms.DB.Models.ModifierSystem.StatGroupMappings;

public class StatGroupMapping
{
    public int Id { get; set; }
    public int StatGroupId { get; set; }
    public int StatModifierId { get; set; }
    public int Modifier { get; set; }
    public bool ScaleWithLevel { get; set; }
    public bool CreationSpecificBonus { get; set; }
    public int? TargetExpressionId { get; set; }
    public StatModifierGroup StatModifierGroup { get; set; } = null!;
    public StatModifier StatModifier { get; set; } = null!;
    public Expression? Expression { get; set; }
}
