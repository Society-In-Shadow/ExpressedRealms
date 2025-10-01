namespace ExpressedRealms.Expressions.UseCases.StatModifiers.Edit;

public class EditStatModifierModel
{
    public int Id { get; set; }
    public int StatModifierGroupId { get; set; }
    public bool ScaleWithLevel { get; set; }
    public int Modifier { get; set; }
    public bool CreationSpecificBonus { get; set; }
    public int StatModifierId { get; set; }
}
