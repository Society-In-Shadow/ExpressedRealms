using Dapr.Client;
using ExpressedRealms.Shared.AzureKeyVault.Secrets.Config;
using Microsoft.Extensions.Caching.Memory;

namespace ExpressedRealms.Shared.AzureKeyVault;

public sealed class KeyVaultManager : IKeyVaultManager
{
    private readonly DaprClient? _secretClient;
    private readonly IMemoryCache _memoryCache;

    public KeyVaultManager(IMemoryCache memoryCache)
    {
        _secretClient = new DaprClientBuilder().Build();
        _memoryCache = memoryCache;
    }

    public async Task<string?> GetSecret(IKeyVaultSecret secretName)
    {
        if (_memoryCache.TryGetValue(secretName, out string? cachedSecret))
            return cachedSecret;

        const string secretStoreName = "azure-key-vault";

        var keyValueSecret = (
            await _secretClient!.GetSecretAsync(secretStoreName, secretName.Name)
        ).Values.FirstOrDefault();

        cachedSecret =
            keyValueSecret
            ?? throw new KeyNotFoundException($"Secret {secretName.Name} not found in Key Vault");

        _memoryCache.Set(secretName, cachedSecret, TimeSpan.FromHours(6));

        return cachedSecret;
    }
}
