namespace ExpressedRealms.Repositories.Expressions.ExpressionTextSections.DTOs;

public class EditExpressionHierarchyDto
{
    public int ExpressionId { get; set; }
    public List<EditExpressionHierarchyItemDto> Items { get; set; } = null!;
}
