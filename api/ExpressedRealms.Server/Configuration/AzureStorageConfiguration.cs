using Azure.Identity;
using Azure.Storage.Blobs;
using ExpressedRealms.Authentication.AzureKeyVault;
using ExpressedRealms.Authentication.AzureKeyVault.Secrets;
using Microsoft.AspNetCore.DataProtection;

namespace ExpressedRealms.Server.Configuration;

public static class AzureStorageConfiguration
{
    public static async Task SetupBlobStorage(
        this WebApplicationBuilder builder,
        EarlyKeyVaultManager manager
    )
    {
        // Since we are in a container, we need to keep track of the data keys manually

        var blobStorageEndpoint = await manager.GetSecret(ConnectionStrings.BlobStorage);

        BlobClient blobClient;
        if (builder.Environment.IsDevelopment())
        {
            BlobContainerClient blobContainerClient = new BlobContainerClient(
                blobStorageEndpoint,
                "dataprotection-keys"
            );
            await blobContainerClient.CreateIfNotExistsAsync();
            blobClient = blobContainerClient.GetBlobClient("dataprotection-keys.xml");
        }
        else
        {
            var blobServiceClient = new BlobServiceClient(
                new Uri(blobStorageEndpoint),
                new DefaultAzureCredential()
            );
            var containerClient = blobServiceClient.GetBlobContainerClient("dataprotection-keys");
            blobClient = containerClient.GetBlobClient("dataprotection-keys.xml");
        }

        builder.Services.AddDataProtection().PersistKeysToAzureBlobStorage(blobClient);
    }
}
