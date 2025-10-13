using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace ExpressedRealms.Server.Configuration.ApplicationInsights;

public class RouteTemplateTelemetryInitializer(IHttpContextAccessor httpContextAccessor) : ITelemetryInitializer
{
    public void Initialize(ITelemetry telemetry)
    {
        if (telemetry is not RequestTelemetry requestTelemetry) return;
        var context = httpContextAccessor.HttpContext;
        var endpoint = context?.GetEndpoint() as RouteEndpoint;

        if (endpoint == null) return;
        
        // Replace the name with the route pattern, e.g. "POST /characters/{id}/blessings"
        requestTelemetry.Name = $"{context!.Request.Method} {endpoint.RoutePattern.RawText}";
        // Optional: add the raw route as a property for queries
        requestTelemetry.Properties["RouteTemplate"] = endpoint.RoutePattern.RawText;
    }
}