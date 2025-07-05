using ExpressedRealms.Expressions.API.ExpressionSubSectionEndpoints.DTOs;

namespace ExpressedRealms.Expressions.API.ExpressionSubSectionEndpoints.Responses;

public class ExpressionBaseResponse
{
    public List<ExpressionSectionDto> ExpressionSections { get; set; } = new();
    public bool CanEditPolicy { get; set; }
    public bool ShowPowersTab { get; set; }
}
