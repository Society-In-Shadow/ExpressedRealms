using ExpressedRealms.Authentication.AzureKeyVault.Secrets.Config;

namespace ExpressedRealms.Authentication.AzureKeyVault.Secrets;

public static class GeneralConfig
{
    public static readonly KeyVaultSecret FrontEndBaseUrl = new("FRONT-END-BASE-URL");
    public static readonly KeyVaultSecret CookieDomain = new("CLIENT-COOKIE-DOMAIN");
}
