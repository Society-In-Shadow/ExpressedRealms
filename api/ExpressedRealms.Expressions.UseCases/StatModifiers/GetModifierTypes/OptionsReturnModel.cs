namespace ExpressedRealms.Expressions.UseCases.StatModifiers.GetModifierTypes;

public class OptionsReturnModel
{
    public List<ModifierTypesReturnModel> ModifierTypes { get; set; } = new();
    public List<ExpressionReturnModel> Expressions { get; set; } = new();
}
