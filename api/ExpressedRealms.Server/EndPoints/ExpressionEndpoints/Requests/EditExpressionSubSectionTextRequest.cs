namespace ExpressedRealms.Server.EndPoints.ExpressionEndpoints.Requests;

public class EditExpressionSubSectionTextRequest
{
    public string Name { get; set; } = null!;
    public string Content { get; set; } = null!;
    public int SectionTypeId { get; set; }
}
