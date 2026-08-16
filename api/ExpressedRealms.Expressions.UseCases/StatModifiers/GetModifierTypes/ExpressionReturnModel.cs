namespace ExpressedRealms.Expressions.UseCases.StatModifiers.GetModifierTypes;

public class ExpressionReturnModel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public List<ProgressionPath> ProgressionPaths { get; set; } = [];
}
