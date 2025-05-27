using ExpressedRealms.Authentication.AzureKeyVault;
using ExpressedRealms.Authentication.AzureKeyVault.Secrets;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ExpressedRealms.FeatureFlags.Configuration;

public class FliptHealthCheck : IHealthCheck
{
    private HttpClient _httpClient;
    private readonly IKeyVaultManager _keyVaultManager;

    public FliptHealthCheck(IKeyVaultManager keyvaultManager)
    {
        _keyVaultManager = keyvaultManager;
    }

    private async Task SetupClient()
    {
        _httpClient = new()
        {
            BaseAddress = new Uri(
                await _keyVaultManager.GetSecret(FeatureFlagSettings.FeatureFlagUrl)
            ),
        };
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            await SetupClient();

            var response = await _httpClient.GetAsync("/health", cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                return HealthCheckResult.Healthy("Flipt service is healthy");
            }

            return HealthCheckResult.Degraded(
                $"Flipt health check failed with status code: {response.StatusCode}"
            );
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Flipt service is unreachable", ex);
        }
    }
}
