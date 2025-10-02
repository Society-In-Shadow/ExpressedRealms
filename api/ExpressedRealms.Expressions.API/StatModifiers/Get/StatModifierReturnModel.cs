namespace ExpressedRealms.Expressions.API.StatModifiers.Get;

public class StatModifierReturnModel
{
    public int Id { get; set; }
    public int StatModifierId { get; set; }
    public int Modifier { get; set; }
    public bool ScaleWithLevel { get; set; }
    public bool CreationSpecificBonus { get; set; }
    public int? TargetExpressionId { get; set; }
}
