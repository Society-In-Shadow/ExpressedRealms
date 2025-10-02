namespace ExpressedRealms.Expressions.API.StatModifiers.GetModifierTypes;

public class StatModifiersResponse
{
    public List<ListItem> ModifierTypes { get; set; } = new();
    public List<ListItem> Expressions { get; set; } = new();
}
