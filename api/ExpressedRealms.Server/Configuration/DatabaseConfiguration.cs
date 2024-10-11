using Azure.Core;
using Azure.Identity;
using ExpressedRealms.DB;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace ExpressedRealms.Server.Configuration;

public static class DatabaseConfiguration
{
    public static void AddDatabaseConnection(this WebApplicationBuilder builder, string connectionString)
    {
        if (!string.IsNullOrEmpty(connectionString))
        {
            // Register DbContext with reuse of the existing services
            builder.Services.AddDbContext<ExpressedRealmsDbContext>((_, options) =>
            {
                options.UseNpgsql(connectionString, postgresOptions =>
                {
                    postgresOptions.MigrationsHistoryTable("_EfMigrations", "efcore");
                });
            });

            return;
        }
        
        // Assuming these services are registered once and reused
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(Environment.GetEnvironmentVariable("AZURE_POSTGRESSQL_CONNECTIONSTRING"));

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
            });

        // Build the data source once and reuse it across DbContext instances
        var dataSource = dataSourceBuilder.Build();
    
        // Register DbContext with reuse of the existing services
        builder.Services.AddDbContext<ExpressedRealmsDbContext>((_, options) =>
        {
            options.UseNpgsql(dataSource, postgresOptions =>
            {
                postgresOptions.MigrationsHistoryTable("_EfMigrations", "efcore");
            });
        });
    }
}