namespace ExpressedRealms.Repositories.Expressions.ExpressionTextSections.DTOs;

public class GetExpressionTextSectionDto
{
    public int Id { get; init; }
    public string Name { get; set; } = null!;
    public string Content { get; set; }
    public int? ParentId { get; set; }
    public int SectionTypeId { get; set; }
    public List<SectionTypeDto> SectionTypes { get; set; } = null!;
    public List<ExpressionSectionDto> AvailableParents { get; set; }
}
