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
            var keyVaultUri = Environment.GetEnvironmentVariable("AZURE_KEYVAULT_RESOURCEENDPOINT");
            if (string.IsNullOrEmpty(keyVaultUri) || !Uri.IsWellFormedUriString(keyVaultUri, UriKind.Absolute))
            {
                throw new InvalidOperationException("The Azure Key Vault endpoint URI is not valid. Ensure 'AZURE_KEYVAULT_RESOURCEENDPOINT' is set and correctly formatted.");
            }
            _secretClient = new SecretClient(new Uri(keyVaultUri), new DefaultAzureCredential());
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