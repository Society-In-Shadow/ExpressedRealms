using ExpressedRealms.Expressions.API.ExpressionEndpoints.DTOs;

namespace ExpressedRealms.Expressions.API.ExpressionEndpoints.Responses;

public class ExpressionSectionOptionsResponse
{
    public List<AvailableParentsDto> AvailableParents { get; set; }
    public List<SectionTypeDto> SectionTypes { get; set; }
}
