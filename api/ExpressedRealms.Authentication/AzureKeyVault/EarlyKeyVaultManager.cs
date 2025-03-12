using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Dapr.Client;
using ExpressedRealms.Authentication.AzureKeyVault.Secrets.Config;

namespace ExpressedRealms.Authentication.AzureKeyVault;

public class EarlyKeyVaultManager
{
    private readonly SecretClient _secretClient;
    //private readonly DaprClient _secretClient;
    private readonly bool _isProduction;
    
    public EarlyKeyVaultManager(bool isProduction)
    {
        _isProduction = isProduction;
        if (isProduction)
        {
            //_secretClient = new DaprClientBuilder().Build();
            var keyVaultUri = Environment.GetEnvironmentVariable("AZURE_KEYVAULT_RESOURCEENDPOINT");
            if (string.IsNullOrEmpty(keyVaultUri) || !Uri.IsWellFormedUriString(keyVaultUri, UriKind.Absolute))
            {
                throw new InvalidOperationException("The Azure Key Vault endpoint URI is not valid. Ensure 'AZURE_KEYVAULT_RESOURCEENDPOINT' is set and correctly formatted.");
            }
            _secretClient = new SecretClient(new Uri(keyVaultUri), new DefaultAzureCredential());
        }
    }
    
    public async Task<string> GetSecret(IKeyVaultSecret secretName)
    {
        string secret;
        if (_isProduction)
        {
            // Cache miss: Fetch secret from Azure Key Vault
            // Retrieve the database connection string from the Dapr secret store
            var secretStoreName = "azure-key-vault"; // The name of the configured Dapr secret store
                
            // Cache miss: Fetch secret from Azure Key Vault
            //var keyValueSecret = (await _secretClient.GetSecretAsync(secretStoreName, secretName.Name)).Values.FirstOrDefault();
            var keyValueSecret = await _secretClient.GetSecretAsync(secretStoreName);
            if (keyValueSecret.HasValue == false)
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