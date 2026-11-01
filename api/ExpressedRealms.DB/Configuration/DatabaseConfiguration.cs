using Azure.Core;
using Azure.Identity;
using ExpressedRealms.Shared.AzureKeyVault;
using ExpressedRealms.Shared.AzureKeyVault.Secrets;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace ExpressedRealms.DB.Configuration;

public static class DatabaseConfiguration
{
    public static void AddExpressedRealmsDbContext(this WebApplicationBuilder builder)
    {
        var isDevelopment =
            Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
        if (isDevelopment)
        {
            var connectionString = KeyVaultManager.GetSecret(ConnectionStrings.Database);
            builder.Services.AddDbContext<ExpressedRealmsDbContext>(
                (_, options) =>
                {
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
                }
            );

            return;
        }

        // Needs to be built once outside the db context to prevent pool exhaustion
        var dataSource = GetAzureDataSource();

        builder.Services.AddDbContext<ExpressedRealmsDbContext>(
            (_, options) =>
            {
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
        );
    }

    private static NpgsqlDataSource GetAzureDataSource()
    {
        var azureConnectionString = KeyVaultManager.GetSecret(ConnectionStrings.Database);
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
                    token
                );
                return accessToken.Token;
            }
        );

        // Build the data source once and reuse it across DbContext instances
        var dataSource = dataSourceBuilder.Build();
        return dataSource;
    }
}
