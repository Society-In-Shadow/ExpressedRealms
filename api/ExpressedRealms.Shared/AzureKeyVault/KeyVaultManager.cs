using Dapr.Client;
using ExpressedRealms.Shared.AzureKeyVault.Secrets.Config;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;

namespace ExpressedRealms.Shared.AzureKeyVault;

internal sealed class KeyVaultManager : IKeyVaultManager
{
    private readonly DaprClient? _secretClient;
    private readonly IHostEnvironment _environment;
    private readonly IMemoryCache _memoryCache;

    public KeyVaultManager(IMemoryCache memoryCache, IHostEnvironment environment)
    {
        if (!environment.IsDevelopment())
        {
            _secretClient = new DaprClientBuilder().Build();
        }
        _memoryCache = memoryCache;
        _environment = environment;
    }

    public async Task<string?> GetSecret(IKeyVaultSecret secretName)
    {
        if (_memoryCache.TryGetValue(secretName, out string? cachedSecret))
            return cachedSecret;

        if (_environment.IsDevelopment())
        {
            cachedSecret = Environment.GetEnvironmentVariable(secretName.Name);
        }
        else
        {
            var secretStoreName = "azure-key-vault";

            var keyValueSecret = (
                await _secretClient!.GetSecretAsync(secretStoreName, secretName.Name)
            ).Values.FirstOrDefault();

            cachedSecret =
                keyValueSecret
                ?? throw new KeyNotFoundException(
                    $"Secret {secretName.Name} not found in Key Vault"
                );
        }

        _memoryCache.Set(secretName, cachedSecret, TimeSpan.FromHours(6));

        return cachedSecret;
    }
}
