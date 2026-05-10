namespace ExpressedRealms.Expressions.API.ExpressionSubSectionEndpoints.Requests;

public class CreateExpressionSubSectionTextRequest
{
    public string Name { get; set; } = null!;
    public string Content { get; set; } = null!;
    public int SectionTypeId { get; set; }
    public int? ParentId { get; set; }
}
