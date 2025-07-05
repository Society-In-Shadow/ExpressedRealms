namespace ExpressedRealms.Expressions.API.ExpressionSubSectionEndpoints.Responses;

public class EditExpressionSectionResponse
{
    public int Id { get; init; }
    public string Name { get; set; } = null!;
    public required string Content { get; set; }
    public int? ParentId { get; set; }
    public int SectionTypeId { get; set; }
}
