namespace ExpressedRealms.Expressions.API.ExpressionEndpoints.UpdateHierarchy;

public class EditExpressionHierarchyItemRequest
{
    public int ExpressionId { get; set; }
    public List<EditExpressionHierarchyItemReqestDto> Items { get; set; } = new();
}
