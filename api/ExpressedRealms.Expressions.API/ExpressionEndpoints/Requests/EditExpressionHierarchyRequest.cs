namespace ExpressedRealms.Expressions.API.ExpressionEndpoints.Requests;

public class EditExpressionHierarchyItemRequest
{
    public int ExpressionId { get; set; }
    public List<EditExpressionHierarchyItemReqestDto> Items { get; set; } = new();
}
