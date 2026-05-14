namespace ExpressedRealms.Expressions.API.StatModifiers.Create;

public class CreateStatModifier
{
    public int SourceId { get; set; }
    public bool ScaleWithLevel { get; set; }
    public int Modifier { get; set; }
    public bool CreationSpecificBonus { get; set; }
    public int StatModifierId { get; set; }
    public int? TargetExpressionId { get; set; }
}
