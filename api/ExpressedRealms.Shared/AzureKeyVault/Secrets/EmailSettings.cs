using ExpressedRealms.Shared.AzureKeyVault.Secrets.Config;

namespace ExpressedRealms.Shared.AzureKeyVault.Secrets;

public static class EmailSettings
{
    public static readonly KeyVaultSecret Postmark = new("POSTMARK-API-KEY");
    public static readonly KeyVaultSecret NoReplyEmail = new("NO-REPLY-EMAIL");
    public static readonly KeyVaultSecret FrontEndBaseUrl = new("FRONT-END-BASE-URL");
}
