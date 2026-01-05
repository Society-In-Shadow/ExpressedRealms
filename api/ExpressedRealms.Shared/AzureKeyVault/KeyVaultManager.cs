using ExpressedRealms.Shared.AzureKeyVault.Secrets.Config;
using Microsoft.Extensions.Configuration;

namespace ExpressedRealms.Shared.AzureKeyVault;

public static class KeyVaultManager
{
    private static IConfiguration? _configuration;
    
    public static void Initialize(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public static string GetSecret(IKeyVaultSecret secretName)
    {
        if(_configuration is null)
            throw new InvalidOperationException("KeyVaultManager must be initialized before use.");
        
        var keyValueSecret = _configuration[secretName.Name];

        return keyValueSecret
            ?? throw new KeyNotFoundException($"Secret {secretName.Name} not found");
    }

    public static bool IsSecretSet(IKeyVaultSecret secretName)
    {
        if(_configuration is null)
            throw new InvalidOperationException("KeyVaultManager must be initialized before use.");
        
        var keyValueSecret = _configuration[secretName.Name];

        return !string.IsNullOrWhiteSpace(keyValueSecret);
    }
}
