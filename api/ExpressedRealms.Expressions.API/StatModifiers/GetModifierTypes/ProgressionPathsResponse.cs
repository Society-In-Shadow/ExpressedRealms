namespace ExpressedRealms.Expressions.API.StatModifiers.GetModifierTypes;

public class StatModifiersResponse
{
    public List<ListItem> ModifierTypes { get; set; } = new();
    public List<ExpressionReturnModel> Expressions { get; set; } = new();
}
