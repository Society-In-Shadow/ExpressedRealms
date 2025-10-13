using ExpressedRealms.Authentication.AzureKeyVault;
using ExpressedRealms.Authentication.AzureKeyVault.Secrets;
using Microsoft.ApplicationInsights.Channel;
using Serilog;

namespace ExpressedRealms.Server.Configuration.ApplicationInsights;

public static class ApplicationInsightConfiguration
{
    public static async Task SetupApplicationInsights(
        this WebApplicationBuilder builder,
        EarlyKeyVaultManager keyVaultManager
    )
    {
        var logger = new LoggerConfiguration().MinimumLevel.Information().WriteTo.Console();

        if (builder.Environment.IsDevelopment())
        {
            var testAppInsightsLocally = await keyVaultManager.GetSecret(
                GeneralConfig.TestAppInsightsLocally
            );
            if (testAppInsightsLocally.Equals("true", StringComparison.InvariantCultureIgnoreCase))
            {
                builder.Services.AddSingleton<ITelemetryChannel, DebugTelemetryChannel>();
                builder.Services.AddApplicationInsightsTelemetry();
            }
        }
        else
        {
            var appInsightsConnectionString = await keyVaultManager.GetSecret(
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
        }

        Log.Logger = logger.CreateLogger();
        builder.Host.UseSerilog();
    }
}
