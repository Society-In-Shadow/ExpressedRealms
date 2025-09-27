namespace ExpressedRealms.Expressions.UseCases.ProgressionPaths.Edit;

public class EditProgressionPathModel
{
    public int Id { get; set; }
    public int ExpressionId { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
}
