using Audit.Core;
using Azure.Identity;
using ExpressedRealms.Admin.API.Configuration;
using ExpressedRealms.Admin.UseCases.Configuration;
using ExpressedRealms.Authentication.Configuration;
using ExpressedRealms.Authentication.PermissionCollection.PermissionManager;
using ExpressedRealms.Blessings.API.Configuration;
using ExpressedRealms.Blessings.UseCases.Configuration;
using ExpressedRealms.Characters.API.Configuration;
using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.UseCases.Configuration;
using ExpressedRealms.DB;
using ExpressedRealms.DB.Configuration;
using ExpressedRealms.Email;
using ExpressedRealms.Events.API.API.Configuration;
using ExpressedRealms.Events.API.UseCases.Configuration;
using ExpressedRealms.Expressions.API.Configuration;
using ExpressedRealms.Expressions.Repository;
using ExpressedRealms.Expressions.UseCases.Configuration;
using ExpressedRealms.FeatureFlags.Configuration;
using ExpressedRealms.FeatureFlags.FeatureManager;
using ExpressedRealms.Knowledges.API.Configuration;
using ExpressedRealms.Knowledges.UseCases.Configuration;
using ExpressedRealms.Powers.API.Configuration;
using ExpressedRealms.Powers.Repository;
using ExpressedRealms.Powers.UseCases.Configuration;
using ExpressedRealms.Repositories.Shared.ExternalDependencies;
using ExpressedRealms.Server.Configuration;
using ExpressedRealms.Server.Configuration.ApplicationInsights;
using ExpressedRealms.Server.Configuration.UserRoles;
using ExpressedRealms.Server.CronJobs;
using ExpressedRealms.Server.DependencyInjections;
using ExpressedRealms.Server.EndPoints;
using ExpressedRealms.Server.EndPoints.PlayerEndpoints;
using ExpressedRealms.Server.Shared.Extensions;
using ExpressedRealms.Shared.AzureKeyVault;
using ExpressedRealms.Shared.AzureKeyVault.Secrets;
using ExpressedRealms.Shared.Configuration;
using FluentValidation;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serilog;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

// Disable log warning, we want a bunch of them here
#pragma warning disable S6664

try
{
    Log.Information("Setting Up Loggers");
    var earlyLogger = new LoggerConfiguration().MinimumLevel.Information().WriteTo.Console();
    Log.Logger = earlyLogger.CreateLogger();

    Log.Information("Setting Up Web App");
    var builder = WebApplication.CreateBuilder(args);

    if (builder.Environment.IsProduction())
    {
        var azureKeyVaultUri = Environment.GetEnvironmentVariable(
            ConnectionStrings.AzureKeyVault.Name
        );
        if (string.IsNullOrWhiteSpace(azureKeyVaultUri))
        {
            throw new KeyNotFoundException(
                $"Secret {ConnectionStrings.AzureKeyVault.Name} not found"
            );
        }
        builder.Configuration.AddAzureKeyVault(
            new Uri(azureKeyVaultUri),
            new DefaultAzureCredential()
        );
    }
    else if (Environment.GetEnvironmentVariable("IS_DOCKER_COMPOSE") == "true")
    {
        Log.Information("Setting Up Docker Compose");

        builder.Configuration.AddJsonFile($"appsettings.docker.json", optional: false);
    }

    KeyVaultManager.Initialize(builder.Configuration);

    Log.Information("Setup In Memory Cache");
    builder.Services.AddMemoryCache();

    Log.Information("Setup Azure Key Vault");
    builder.Services.AddAuthenticationInjections();

    Log.Information("Setup Application Insights");
    builder.SetupApplicationInsights();

    Log.Information("Setup Azure Storage Blob");
    await builder.SetupBlobStorage();

    Log.Information("Add in Health Checks");
    builder.Services.AddHealthChecks();

    Log.Information("Adding DB Context");
    builder.AddExpressedRealmsDbContext();

    Log.Information("Adding Redis Cache");
    await builder.AddRedisConnection(builder.Environment.IsProduction());

    Log.Information("Add Quartz / Cron Scheduler");
    builder.SetupQuartz();

    Log.Information("Setting Up Authentication and Identity");
    SecurityConfiguration.SetupAuthenticationAndIdentity(builder);

    Log.Information("Adding OpenAPI Support and Swagger Generation");

    builder.Services.AddOpenApi();

    Log.Information("Adding Email Dependencies");
    builder.Services.AddEmailDependencies();

    Log.Information("Configuring various things");
    builder.Services.AddValidatorsFromAssemblyContaining<Program>();
    builder.Services.AddFluentValidationAutoValidation();
    builder.Services.AddSingleton<ITelemetryInitializer, RouteTemplateTelemetryInitializer>();

    builder.Services.AddHttpContextAccessor();
    // https://stackoverflow.com/questions/64122616/cancellation-token-injection/77342914#77342914
    builder.Services.AddScoped(
        typeof(CancellationToken),
        serviceProvider =>
        {
            IHttpContextAccessor httpContext =
                serviceProvider.GetRequiredService<IHttpContextAccessor>();
            return httpContext.HttpContext?.RequestAborted ?? CancellationToken.None;
        }
    );
    builder.Services.AddScoped<IUserContext, UserContext>();
    builder.Services.AddCharacterRepositoryInjections();
    builder.Services.AddExpressionRepositoryInjections();
    builder.Services.AddAdminInjections();
    builder.Services.AddPowerRepositoryInjections();
    builder.Services.AddKnowledgesInjections();
    builder.Services.AddExpressionTextSectionInjections();
    builder.Services.AddBlessingInjections();
    builder.Services.AddPowerUseCaseConfiguration();
    builder.Services.AddCharacterUseCaseInjections();
    builder.Services.AddEventUseCaseInjections();
    builder.Services.AddSharedInjections();
    builder.Services.AddFeatureFlagInjections();

    Log.Information("Building the App");
    var app = builder.Build();

    app.UseSerilogRequestLogging();

    Configuration.AddCustomAction(
        ActionType.OnScopeCreated,
        scope =>
        {
            var httpContext = app.Services.GetService<IHttpContextAccessor>();
            // This will only ever be empty when the user isn't logged in (think creating a new user)
            if (
                httpContext is null
                || httpContext.HttpContext is null
                || !httpContext.HttpContext.User.Identity.IsAuthenticated
            )
            {
                return;
            }
            scope.Event.CustomFields.Add("UserId", httpContext.HttpContext?.User.GetUserId());
        }
    );

    // Migrate latest database changes during startup
    Log.Information("Checking if Migrations Need to Be Run");
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ExpressedRealmsDbContext>();

        if ((await dbContext.Database.GetPendingMigrationsAsync()).Any())
        {
            Log.Information("DB is missing migrations, running them now");
            await dbContext.Database.MigrateAsync();

            // Need a way to manually run sql scripts, will assume they need to be updated
            // These need to come at the end
            // Updating a script three times will cause issues if migrations need table changes
            Log.Information("Successfully ran all migrations!");
        }
        else
        {
            Log.Information("No Migrations are needed");
        }
    }

    // Make sure that table changes are applied before updating flags and permissions
    Log.Information("Updating Feature Flags");
    using (var scope = app.Services.CreateScope())
    {
        var featureToggleManager =
            scope.ServiceProvider.GetRequiredService<IFeatureToggleManager>();
        await featureToggleManager.UpdateFeatureToggles();
    }

    Log.Information("Updating Permissions");
    using (var scope = app.Services.CreateScope())
    {
        var permissionManager = scope.ServiceProvider.GetRequiredService<IPermissionManager>();
        await permissionManager.UpdatePermissions();
    }

    if (app.Environment.IsProduction())
    {
        Log.Information("Setting Up Forwarded Headers");
        app.UseForwardedHeaders();
    }

    // Seed Roles
    await app.ConfigureUserRoles();

    app.UseDefaultFiles();
    app.UseStaticFiles();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        Log.Information("Setting Up Scalar API");
        app.MapOpenApi();
        app.MapScalarApiReference();
    }

    Log.Information("Adding Health Check Endpoint");

    app.MapHealthChecks("health");

    Log.Information("Adding in Security Related Things");

    if (app.Environment.IsDevelopment())
    {
        app.UseHttpsRedirection();
    }

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseAntiforgery();

    Log.Information("Adding endpoints");
    app.AddAuthEndPoints();
    app.AddTestingEndPoints();
    app.AddPlayerEndPoints();
    app.AddNavigationEndpoints();
    app.AddExpressionEndPoints();
    app.ConfigureAdminEndPoints();
    app.AddPowerEndPoints();
    app.AddCharacterApiEndPoints();
    app.ConfigureBlessingEndPoints();
    app.ConfigureKnowledgeEndPoints();
    app.ConfigureEventsEndPoints();

    app.MapFallbackToFile("index.html");
    Log.Information("Starting Web API");
    await app.RunAsync();
}
catch (Exception ex)
{
    // https://github.com/dotnet/efcore/issues/29809#issuecomment-1345132260
    if (ex is HostAbortedException)
    {
        Log.Information("EF Core Migration Build was detected.  Catching and closing out.");
        return;
    }

    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    await Log.CloseAndFlushAsync();
}
