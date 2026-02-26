using Azure.Core;
using Azure.Identity;
using ExpressedRealms.Shared.AzureKeyVault;
using ExpressedRealms.Shared.AzureKeyVault.Secrets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql;

namespace ExpressedRealms.DB.Configuration;

public static class DatabaseConfiguration
{
    public static void AddExpressedRealmsDbConnection(this DbContextOptionsBuilder options)
    {
        var isDevelopment =
            Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
        if (isDevelopment)
        {
            var connectionString = KeyVaultManager.GetSecret(ConnectionStrings.Database);

            options
                .UseNpgsql(
                    connectionString,
                    postgresOptions =>
                    {
                        postgresOptions.MigrationsHistoryTable("_EfMigrations", "efcore");
                    }
                )
                .ReplaceService<IHistoryRepository, CamelCaseHistoryContext>()
                .UseSnakeCaseNamingConvention();

            return;
        }

        var azureConnectionString = KeyVaultManager.GetSecret(ConnectionStrings.Database);
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

        options
            .UseNpgsql(
                dataSource,
                postgresOptions =>
                {
                    postgresOptions.MigrationsHistoryTable("_EfMigrations", "efcore");
                }
            )
            .ReplaceService<IHistoryRepository, CamelCaseHistoryContext>()
            .UseSnakeCaseNamingConvention();
    }
}
