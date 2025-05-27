using ExpressedRealms.Authentication.AzureKeyVault.Secrets.Config;

namespace ExpressedRealms.Authentication.AzureKeyVault.Secrets;

public class FeatureFlagSettings
{
    public static readonly KeyVaultSecret FeatureFlagUrl = new("FEATURE-FLAG-URL");
}
