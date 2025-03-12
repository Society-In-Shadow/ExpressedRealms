using Dapr.Client;
using ExpressedRealms.Authentication.AzureKeyVault.Secrets.Config;

namespace ExpressedRealms.Authentication.AzureKeyVault;

public class EarlyKeyVaultManager
{
    private readonly DaprClient? _secretClient;
    private readonly bool _isProduction;

    public EarlyKeyVaultManager(bool isProduction)
    {
        _isProduction = isProduction;
        if (isProduction)
        {
            _secretClient = new DaprClientBuilder().Build();
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
            var keyValueSecret = (
                await _secretClient.GetSecretAsync(secretStoreName, secretName.Name)
            ).Values.FirstOrDefault();
            if (keyValueSecret is null)
                throw new Exception($"Secret {secretName.Name} not found in Key Vault");

            secret = keyValueSecret;
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
