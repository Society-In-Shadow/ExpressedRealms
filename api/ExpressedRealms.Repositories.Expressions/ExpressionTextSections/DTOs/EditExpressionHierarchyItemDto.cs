namespace ExpressedRealms.Repositories.Expressions.ExpressionTextSections.DTOs;

public class EditExpressionHierarchyItemDto
{
    public int Id { get; set; }
    public int? ParentId { get; set; }
    public int SortOrder { get; set; }
}
