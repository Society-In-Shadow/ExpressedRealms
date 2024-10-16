namespace ExpressedRealms.Repositories.Expressions.ExpressionTextSections.DTOs;

public record EditExpressionTextSectionDto
{
    public int Id { get; init; }
    public string Name { get; set; } = null!;
    public string Content { get; set; } = null!;
    public int ExpressionId { get; set; }
    public int SectionTypeId { get; set; }
    public int? ParentId { get; set; }
}
