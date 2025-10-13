using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;

namespace ExpressedRealms.Server.Configuration.ApplicationInsights;

// Intended for debugging App Insights locally
public class DebugTelemetryChannel : ITelemetryChannel
{
    public void Send(ITelemetry item)
    {
        switch (item)
        {
            case RequestTelemetry req:
                Console.WriteLine(
                    $"[RequestTelemetry] Name: {req.Name}, "
                        + $"Success: {req.Success}, "
                        + $"ResponseCode: {req.ResponseCode}, "
                        + $"Duration: {req.Duration}, "
                        + $"Url: {req.Url}"
                );
                if (req.Properties?.Count > 0)
                {
                    Console.WriteLine("  Properties:");
                    foreach (var kvp in req.Properties)
                        Console.WriteLine($"    {kvp.Key}: {kvp.Value}");
                }
                break;

            case DependencyTelemetry dep:
                Console.WriteLine(
                    $"[DependencyTelemetry] Name: {dep.Name}, "
                        + $"Target: {dep.Target}, "
                        + $"Type: {dep.Type}, "
                        + $"Duration: {dep.Duration}, "
                        + $"Success: {dep.Success}"
                );
                break;

            case TraceTelemetry trace:
                Console.WriteLine(
                    $"[TraceTelemetry] Message: {trace.Message}, "
                        + $"Severity: {trace.SeverityLevel}"
                );
                break;

            default:
                Console.WriteLine($"[Telemetry] Type: {item.GetType().Name}");
                break;
        }
    }

    public void Flush() { }

    public bool? DeveloperMode { get; set; } = true;
    public string EndpointAddress { get; set; }

    public void Dispose() { }
}
