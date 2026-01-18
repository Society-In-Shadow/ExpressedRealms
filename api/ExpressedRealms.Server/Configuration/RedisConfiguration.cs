using ExpressedRealms.Shared.AzureKeyVault;
using ExpressedRealms.Shared.AzureKeyVault.Secrets;
using StackExchange.Redis;

namespace ExpressedRealms.Server.Configuration;

public static class RedisConfiguration
{
    public static async Task AddRedisConnection(
        this WebApplicationBuilder builder,
        bool isProduction
    )
    {
        var options = ConfigurationOptions.Parse(
            KeyVaultManager.GetSecret(ConnectionStrings.RedisConnectionString)
        );
        options.AbortOnConnectFail = false;

        if (builder.Environment.IsProduction())
        {
            options.Ssl = true;
            //await options.ConfigureForAzureWithTokenCredentialAsync(new DefaultAzureCredential());
        }

        var multiplexer = await ConnectionMultiplexer.ConnectAsync(options);

        builder.Services.AddSingleton<IConnectionMultiplexer>(multiplexer);
    }
}
