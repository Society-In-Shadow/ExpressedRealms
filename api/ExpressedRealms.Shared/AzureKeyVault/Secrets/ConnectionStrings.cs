using ExpressedRealms.Shared.AzureKeyVault.Secrets.Config;

namespace ExpressedRealms.Shared.AzureKeyVault.Secrets;

public static class ConnectionStrings
{
    public static readonly KeyVaultSecret Database = new("POSTGRES-CONNECTION-STRING");
    public static readonly KeyVaultSecret ApplicationInsights = new(
        "APPLICATION-INSIGHTS-CONNECTION-STRING"
    );
    public static readonly KeyVaultSecret BlobStorage = new("AZURE-STORAGEBLOB-RESOURCEENDPOINT");
    // This is formatted differently due to it being in the github workflow file
    // do not blindly copy this style, use "-" instead
    public static readonly KeyVaultSecret AzureKeyVault = new("AZURE_KEYVAULT_RESOURCEENDPOINT");
    public static readonly KeyVaultSecret RedisConnectionString = new(
        "AZURE-REDIS-CONNECTIONSTRING"
    );
}
