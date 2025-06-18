using ExpressedRealms.Server.EndPoints.ExpressionEndpoints.DTOs;

namespace ExpressedRealms.Server.EndPoints.ExpressionEndpoints.Responses;

public class ExpressionBaseResponse
{
    public List<ExpressionSectionDTO> ExpressionSections { get; set; }
    public bool CanEditPolicy { get; set; }
    public bool ShowPowersTab { get; set; }
}
