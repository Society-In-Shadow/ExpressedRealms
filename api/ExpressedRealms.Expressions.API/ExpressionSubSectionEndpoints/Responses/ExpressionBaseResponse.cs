using ExpressedRealms.Expressions.API.ExpressionSubSectionEndpoints.DTOs;

namespace ExpressedRealms.Expressions.API.ExpressionSubSectionEndpoints.Responses;

public class ExpressionBaseResponse
{
    public List<ExpressionSectionDTO> ExpressionSections { get; set; }
    public bool CanEditPolicy { get; set; }
    public bool ShowPowersTab { get; set; }
}
