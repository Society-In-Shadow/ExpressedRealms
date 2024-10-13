using ExpressedRealms.Server.EndPoints.NavigationEndpoints.DTOs;

namespace ExpressedRealms.Server.EndPoints.NavigationEndpoints.Responses;

public class ExpressionMenuResponse
{
    public bool CanEdit { get; set; }
    public List<ExpressionMenuItem> MenuItems { get; set; }
}
