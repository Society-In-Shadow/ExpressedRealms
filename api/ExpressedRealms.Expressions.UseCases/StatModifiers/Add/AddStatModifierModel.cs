namespace ExpressedRealms.Expressions.UseCases.StatModifiers.Add;

public class AddStatModifierModel
{
    public SourceTableEnum SourceTable { get; set; }
    public int SourceId { get; set; }
    public int? StatModifierGroupId { get; set; }
    public bool ScaleWithLevel { get; set; }
    public int Modifier { get; set; }
    public bool CreationSpecificBonus { get; set; }
    public int StatModifierId { get; set; }
}
