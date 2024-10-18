using ExpressedRealms.Server.EndPoints.ExpressionEndpoints.DTOs;

namespace ExpressedRealms.Server.EndPoints.ExpressionEndpoints.Responses;

public class ExpressionBaseResponse
{
    public int ExpressionId { get; set; }
    public List<ExpressionSectionDTO> ExpressionSections { get; set; }
}