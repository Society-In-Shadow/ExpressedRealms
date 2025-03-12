using ExpressedRealms.Authentication.AzureKeyVault.Secrets.Config;

namespace ExpressedRealms.Authentication.AzureKeyVault.Secrets;

public static class ConnectionStrings
{
    public static readonly KeyVaultSecret Database = new("POSTGRES-CONNECTION-STRING");
    public static readonly KeyVaultSecret ApplicationInsights = new(
        "APPLICATION-INSIGHTS-CONNECTION-STRING"
    );
    public static readonly KeyVaultSecret BlobStorage = new("AZURE-STORAGEBLOB-RESOURCEENDPOINT");
}
