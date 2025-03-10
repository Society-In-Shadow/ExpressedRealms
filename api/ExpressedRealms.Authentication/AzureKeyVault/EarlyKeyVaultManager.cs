using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using ExpressedRealms.Authentication.AzureKeyVault.Secrets.Config;

namespace ExpressedRealms.Authentication.AzureKeyVault;

public class EarlyKeyVaultManager
{
    
    private readonly SecretClient _secretClient;
    
    public EarlyKeyVaultManager(bool isProduction)
    {
        if (isProduction)
        {
            _secretClient = new SecretClient(new Uri(Environment.GetEnvironmentVariable("AZURE_KEY_VAULT_URL")), new DefaultAzureCredential());
        }
    }
    
    public async Task<string> GetSecret(IKeyVaultSecret secretName, bool isProduction = false)
    {
        string secret;
        if (isProduction)
        {
            // Cache miss: Fetch secret from Azure Key Vault
            var keyValueSecret = await _secretClient.GetSecretAsync(secretName.Name);
            if (!keyValueSecret.HasValue)
                throw new Exception($"Secret {secretName.Name} not found in Key Vault");
        
            secret = keyValueSecret.Value.Value;
        }
        else
        {
            var value = Environment.GetEnvironmentVariable(secretName.Name);
            if (string.IsNullOrEmpty(value))
                throw new Exception($"Secret {secretName.Name} not found in Environment Variables");
            
            secret = value;
        }

        // Store the secret in the cache with expiration
        return secret;
    }
}