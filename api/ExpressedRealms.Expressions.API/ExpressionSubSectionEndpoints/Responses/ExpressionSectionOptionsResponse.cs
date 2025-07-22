using ExpressedRealms.Expressions.API.ExpressionSubSectionEndpoints.DTOs;

namespace ExpressedRealms.Expressions.API.ExpressionSubSectionEndpoints.Responses;

public class ExpressionSectionOptionsResponse
{
    public List<SectionTypeDto> SectionTypes { get; set; } = new();
}
