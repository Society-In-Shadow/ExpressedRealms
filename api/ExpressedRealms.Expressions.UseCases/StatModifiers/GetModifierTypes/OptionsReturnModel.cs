namespace ExpressedRealms.Expressions.UseCases.StatModifiers.GetModifierTypes;

public class OptionsReturnModel
{
    public List<ModifierTypesReturnModel> ModifierTypes { get; set; } = new();
    public List<KeyValuePair<int, string>> Expressions { get; set; } = new();
}
