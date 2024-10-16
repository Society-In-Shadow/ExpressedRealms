namespace ExpressedRealms.Repositories.Expressions.Expressions.DTOs;

public record CreateExpressionTextSectionDto
{
    public string Name { get; set; } = null!;
    public string Content { get; set; } = null!;
    public int ExpressionId { get; set; }
    public int SectionTypeId { get; set; }
    public int? ParentId { get; set; }
}
