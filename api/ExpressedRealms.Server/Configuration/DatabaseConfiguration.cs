using Azure.Core;
using Azure.Identity;
using ExpressedRealms.Authentication.AzureKeyVault;
using ExpressedRealms.Authentication.AzureKeyVault.Secrets;
using ExpressedRealms.DB;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace ExpressedRealms.Server.Configuration;

public static class DatabaseConfiguration
{
    public static async Task AddDatabaseConnection(
        this WebApplicationBuilder builder,
        EarlyKeyVaultManager vaultManager,
        bool isProduction
    )
    {
        if (!isProduction)
        {
            var connectionString = await vaultManager.GetSecret(ConnectionStrings.Database);
            // Register DbContext with reuse of the existing services
            builder.Services.AddDbContext<ExpressedRealmsDbContext>(
                (_, options) =>
                {
                    options.UseNpgsql(
                        connectionString,
                        postgresOptions =>
                        {
                            postgresOptions.MigrationsHistoryTable("_EfMigrations", "efcore");
                        }
                    );
                }
            );

            return;
        }

        var azureConnectionString = await vaultManager.GetSecret(ConnectionStrings.Database);
        // Assuming these services are registered once and reused
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(azureConnectionString);

        // Define the password provider once and reuse
        var sqlServerTokenProvider = new DefaultAzureCredential();
        dataSourceBuilder.UsePasswordProvider(
            passwordProvider: _ =>
            {
                AccessToken accessToken = sqlServerTokenProvider.GetToken(
                    new TokenRequestContext(["https://ossrdbms-aad.database.windows.net/.default"])
                );
                return accessToken.Token;
            },
            passwordProviderAsync: async (_, token) =>
            {
                AccessToken accessToken = await sqlServerTokenProvider.GetTokenAsync(
                    new TokenRequestContext(["https://ossrdbms-aad.database.windows.net/.default"]),
                    token // Pass the cancellation token if needed
                );
                return accessToken.Token;
            }
        );

        // Build the data source once and reuse it across DbContext instances
        var dataSource = dataSourceBuilder.Build();

        // Register DbContext with reuse of the existing services
        builder.Services.AddDbContext<ExpressedRealmsDbContext>(
            (_, options) =>
            {
                options.UseNpgsql(
                    dataSource,
                    postgresOptions =>
                    {
                        postgresOptions.MigrationsHistoryTable("_EfMigrations", "efcore");
                    }
                );
            }
        );
    }
}
