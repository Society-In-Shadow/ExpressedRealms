namespace ExpressedRealms.Expressions.Repository.ExpressionTextSections.DTOs;

public class EditExpressionHierarchyDto
{
    public int ExpressionId { get; set; }
    public List<EditExpressionHierarchyItemDto> Items { get; set; } = null!;
}
