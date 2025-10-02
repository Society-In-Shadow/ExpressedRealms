namespace ExpressedRealms.Expressions.UseCases.StatModifiers.GetModifierTypes;

public class OptionsReturnModel
{
    public List<ModifierTypesReturnModel> ModifierTypes { get; set; }
    public IEnumerable<KeyValuePair<int, string>> Expressions { get; set; }
}
