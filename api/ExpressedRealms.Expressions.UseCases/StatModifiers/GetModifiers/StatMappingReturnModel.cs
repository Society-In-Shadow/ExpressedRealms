namespace ExpressedRealms.Expressions.UseCases.StatModifiers.GetModifiers;

public class StatMappingReturnModel
{
    public int Id { get; set; }
    public int StatModifierId { get; set; }
    public int Modifier { get; set; }
    public bool ScaleWithLevel { get; set; }
    public bool CreationSpecificBonus { get; set; }
}
