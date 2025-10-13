using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace ExpressedRealms.Server.Configuration.ApplicationInsights;

public class RouteTemplateTelemetryInitializer(IHttpContextAccessor httpContextAccessor)
    : ITelemetryInitializer
{
    public void Initialize(ITelemetry telemetry)
    {
        if (telemetry is not RequestTelemetry requestTelemetry)
            return;
        var context = httpContextAccessor.HttpContext;
        var endpoint = context?.GetEndpoint() as RouteEndpoint;

        if (endpoint == null)
            return;

        requestTelemetry.Context.Operation.Name =
            $"{context!.Request.Method} {endpoint.RoutePattern.RawText}";
        requestTelemetry.Properties["RouteTemplate"] = endpoint.RoutePattern.RawText;
    }
}
