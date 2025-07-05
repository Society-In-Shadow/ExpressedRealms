using ExpressedRealms.Expressions.API.ExpressionEndpoints.DTOs;

namespace ExpressedRealms.Expressions.API.ExpressionEndpoints.Responses;

public class ExpressionBaseResponse
{
    public List<ExpressionSectionDTO> ExpressionSections { get; set; }
    public bool CanEditPolicy { get; set; }
    public bool ShowPowersTab { get; set; }
}
