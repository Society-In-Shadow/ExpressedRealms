namespace ExpressedRealms.Expressions.UseCases.ProgressionPaths.Add;

public class AddProgressionPathModel
{
    public int ExpressionId { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
}
