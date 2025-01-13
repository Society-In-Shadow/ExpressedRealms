using Azure.Identity;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.DataProtection;

namespace ExpressedRealms.Server.Configuration;

public static class AzureStorageConfiguration
{
    public static void SetupBlobStorage(this WebApplicationBuilder builder)
    {
        // Since we are in a container, we need to keep track of the data keys manually
        var blobStorageEndpoint = 
            Environment.GetEnvironmentVariable("AZURE_STORAGEBLOB_RESOURCEENDPOINT") 
            ?? throw new NullReferenceException("Missing AZURE_STORAGEBLOB_RESOURCEENDPOINT environmental variable");
        
        BlobClient blobClient;
        if (builder.Environment.IsDevelopment())
        {
            BlobContainerClient blobContainerClient = new BlobContainerClient(blobStorageEndpoint, "dataprotection-keys");
            blobContainerClient.CreateIfNotExists();
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
