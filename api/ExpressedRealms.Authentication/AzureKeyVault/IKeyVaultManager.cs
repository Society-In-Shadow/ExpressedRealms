using ExpressedRealms.Authentication.AzureKeyVault.Secrets.Config;

namespace ExpressedRealms.Authentication.AzureKeyVault;

public interface IKeyVaultManager
{
    Task<string> GetSecret(IKeyVaultSecret secretName);
}
