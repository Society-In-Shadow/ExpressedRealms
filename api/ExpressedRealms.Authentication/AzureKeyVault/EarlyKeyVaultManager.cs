using Dapr.Client;
using ExpressedRealms.Authentication.AzureKeyVault.Secrets.Config;

namespace ExpressedRealms.Authentication.AzureKeyVault;

public class EarlyKeyVaultManager
{
    private readonly DaprClient? _secretClient;
    private readonly bool _isProduction;
    private const string DaprSecretStoreName = "azure-key-vault";

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
            var keyValueSecret = (
                await _secretClient!.GetSecretAsync(DaprSecretStoreName, secretName.Name)
            ).Values.FirstOrDefault();

            secret =
                keyValueSecret
                ?? throw new KeyNotFoundException($"Secret {secretName.Name} not found in Key Vault");
        }
        else
        {
            var value = Environment.GetEnvironmentVariable(secretName.Name);
            if (string.IsNullOrEmpty(value))
                throw new KeyNotFoundException($"Secret {secretName.Name} not found in Environment Variables");

            secret = value;
        }

        return secret;
    }

    public async Task<bool> IsSecretSet(IKeyVaultSecret secretName)
    {
        if (_isProduction)
        {
            var keyValueSecret = (
                await _secretClient!.GetSecretAsync(DaprSecretStoreName, secretName.Name)
            ).Values.FirstOrDefault();

            return keyValueSecret is not null;
        }

        var value = Environment.GetEnvironmentVariable(secretName.Name);
        return !string.IsNullOrEmpty(value);
    }
}
