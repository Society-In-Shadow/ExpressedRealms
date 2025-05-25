using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ExpressedRealms.FeatureFlags.Configuration;

public class FliptHealthCheck : IHealthCheck
{
    private readonly HttpClient _httpClient;
    private readonly string _fliptUrl;

    public FliptHealthCheck(IConfiguration configuration)
    {
        _httpClient = new HttpClient();
        _fliptUrl = Environment.GetEnvironmentVariable("FEATURE-FLAG-URL")?.TrimEnd('/') 
                    ?? throw new ArgumentNullException("FEATURE-FLAG-URL configuration is missing");
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetAsync(
                $"{_fliptUrl}/health",
                cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                return HealthCheckResult.Healthy("Flipt service is healthy");
            }

            return HealthCheckResult.Degraded($"Flipt health check failed with status code: {response.StatusCode}");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Flipt service is unreachable", ex);
        }
    }
}
