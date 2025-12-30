using ExpressedRealms.Shared.AzureKeyVault.Secrets.Config;

namespace ExpressedRealms.Shared.AzureKeyVault.Secrets;

public static class GeneralConfig
{
    public static readonly KeyVaultSecret FrontEndBaseUrl = new("FRONT-END-BASE-URL");
    public static readonly KeyVaultSecret CookieDomain = new("CLIENT-COOKIE-DOMAIN");
    public static readonly KeyVaultSecret TestAppInsightsLocally = new("TEST-APP-INSIGHTS-LOCALLY");
}
