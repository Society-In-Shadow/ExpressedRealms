using ExpressedRealms.Shared.AzureKeyVault.Secrets.Config;

namespace ExpressedRealms.Shared.AzureKeyVault.Secrets;

public static class FeatureFlagSettings
{
    public static readonly KeyVaultSecret FeatureFlagUrl = new("FEATURE-FLAG-URL");
}
