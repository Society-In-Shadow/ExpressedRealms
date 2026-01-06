using ExpressedRealms.Shared.AzureKeyVault;
using ExpressedRealms.Shared.AzureKeyVault.Secrets;
using Serilog;

namespace ExpressedRealms.Server.Configuration.ApplicationInsights;

public static class ApplicationInsightConfiguration
{
    public static void SetupApplicationInsights(this WebApplicationBuilder builder)
    {
        if (KeyVaultManager.IsSecretSet(ConnectionStrings.ApplicationInsights))
        {
            var logger = new LoggerConfiguration().MinimumLevel.Information().WriteTo.Console();

            var appInsightsConnectionString = KeyVaultManager.GetSecret(
                ConnectionStrings.ApplicationInsights
            );

            logger.WriteTo.ApplicationInsights(
                appInsightsConnectionString,
                TelemetryConverter.Traces
            );
            builder.Services.AddApplicationInsightsTelemetry(
                (options) =>
                {
                    options.ConnectionString = appInsightsConnectionString;
                }
            );

            Log.Logger = logger.CreateLogger();
        }

        builder.Host.UseSerilog();
    }
}
