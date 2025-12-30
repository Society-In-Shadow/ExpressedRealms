using ExpressedRealms.Shared.AzureKeyVault.Secrets.Config;

namespace ExpressedRealms.Shared.AzureKeyVault;

public interface IKeyVaultManager
{
    Task<string?> GetSecret(IKeyVaultSecret secretName);
}
