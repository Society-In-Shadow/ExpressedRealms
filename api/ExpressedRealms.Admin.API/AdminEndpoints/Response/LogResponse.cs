using ExpressedRealms.Server.EndPoints.AdminEndpoints.Dtos;

namespace ExpressedRealms.Server.EndPoints.AdminEndpoints.Response;

public class LogResponse
{
    public List<LogDto> Logs { get; set; }
}
