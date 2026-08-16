namespace ExpressedRealms.Expressions.Repository.Expressions.DTOs;

public class ExpressionInfoForModifiersProjection
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public List<ExpressionPathProjection> ProgressionPaths { get; set; } = [];
}
