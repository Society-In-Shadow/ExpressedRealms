using ExpressedRealms.Expressions.API.ExpressionSubSectionEndpoints.DTOs;

namespace ExpressedRealms.Expressions.API.ExpressionSubSectionEndpoints.Responses;

public class ExpressionSectionOptionsResponse
{
    public List<AvailableParentsDto> AvailableParents { get; set; } = new();
    public List<SectionTypeDto> SectionTypes { get; set; } = new();
}
