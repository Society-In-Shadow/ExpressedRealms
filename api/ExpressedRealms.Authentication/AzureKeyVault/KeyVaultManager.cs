using Dapr.Client;
using ExpressedRealms.Authentication.AzureKeyVault.Secrets.Config;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;

namespace ExpressedRealms.Authentication.AzureKeyVault;

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

    public async Task<string> GetSecret(IKeyVaultSecret secretName)
    {
        // Attempt to get secret from the cache
        if (!_memoryCache.TryGetValue(secretName, out string cachedSecret))
        {
            if (_environment.IsDevelopment())
            {
                cachedSecret = Environment.GetEnvironmentVariable(secretName.Name);
            }
            else
            {
                // Retrieve the database connection string from the Dapr secret store
                var secretStoreName = "azure-key-vault"; // The name of the configured Dapr secret store

                // Cache miss: Fetch secret from Azure Key Vault
                var keyValueSecret = (
                    await _secretClient.GetSecretAsync(secretStoreName, secretName.Name)
                ).Values.FirstOrDefault();
                if (keyValueSecret is null)
                    throw new Exception($"Secret {secretName.Name} not found in Key Vault");

                cachedSecret = keyValueSecret;
            }

            // Store the secret in the cache with expiration
            _memoryCache.Set(secretName, cachedSecret, TimeSpan.FromHours(6));
        }

        return cachedSecret;
    }
}
