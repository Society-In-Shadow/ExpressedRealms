using ExpressedRealms.Server.EndPoints.ExpressionEndpoints.DTOs;

namespace ExpressedRealms.Server.EndPoints.ExpressionEndpoints.Responses;

public class ExpressionSectionOptionsResponse
{
    public List<AvailableParentsDto> AvailableParents { get; set; }
    public List<SectionTypeDto> SectionTypes { get; set; }
}
