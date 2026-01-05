using Dapr;
using Dapr.Client;
using ExpressedRealms.Shared.AzureKeyVault.Secrets.Config;

namespace ExpressedRealms.Shared.AzureKeyVault;

public class EarlyKeyVaultManager
{
    private readonly DaprClient _secretClient = new DaprClientBuilder().Build();
    private const string DaprSecretStoreName = "azure-key-vault";

    public async Task<string> GetSecret(IKeyVaultSecret secretName)
    {
        var keyValueSecret = (
            await _secretClient.GetSecretAsync(DaprSecretStoreName, secretName.Name)
        ).Values.FirstOrDefault();

        return keyValueSecret
            ?? throw new KeyNotFoundException($"Secret {secretName.Name} not found");
    }

    public async Task<bool> IsSecretSet(IKeyVaultSecret secretName)
    {
        try
        {
            var keyValueSecret = (
                await _secretClient.GetSecretAsync(DaprSecretStoreName, secretName.Name)
            ).Values.FirstOrDefault();

            return !string.IsNullOrWhiteSpace(keyValueSecret);
        }
        catch (DaprException ex)
        {
            if (ex.Message.Contains("not found", StringComparison.OrdinalIgnoreCase))
                return false;

            throw;
        }
    }
}
