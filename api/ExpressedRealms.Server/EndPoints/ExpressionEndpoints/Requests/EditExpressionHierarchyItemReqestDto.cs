namespace ExpressedRealms.Server.EndPoints.ExpressionEndpoints.Requests;

public class EditExpressionHierarchyItemReqestDto
{
    public int Id { get; init; }
    public int? ParentId { get; set; }
    public int SortOrder { get; set; }
    public List<EditExpressionHierarchyItemReqestDto> SubSections { get; set; } = new();
}
