using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using ExpressedRealms.Authentication.AzureKeyVault.Secrets.Config;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;

namespace ExpressedRealms.Authentication.AzureKeyVault;

internal sealed class KeyVaultManager : IKeyVaultManager
{
    private readonly SecretClient _secretClient;
    private readonly IHostEnvironment _environment;
    private readonly IMemoryCache _memoryCache;
    
    public KeyVaultManager(IMemoryCache memoryCache, IHostEnvironment environment )
    {
        if (!environment.IsDevelopment())
        {
            _secretClient = new SecretClient(new Uri(Environment.GetEnvironmentVariable("AZURE_KEYVAULT_RESOURCEENDPOINT")), new DefaultAzureCredential());
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
                // Cache miss: Fetch secret from Azure Key Vault
                var keyValueSecret = await _secretClient.GetSecretAsync(secretName.Name);
                if (!keyValueSecret.HasValue)
                    throw new Exception($"Secret {secretName.Name} not found in Key Vault");
            
                cachedSecret = keyValueSecret.Value.Value;
            }


            // Store the secret in the cache with expiration
            _memoryCache.Set(secretName, cachedSecret, TimeSpan.FromHours(6));
        }

        return cachedSecret;
    }

}