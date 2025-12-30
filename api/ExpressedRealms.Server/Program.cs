using System.Text.Json;
using Audit.Core;
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
using ExpressedRealms.DB.UserProfile.PlayerDBModels.Roles;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;
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
using FluentValidation;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serilog;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

try
{
    Log.Information("Setting Up Loggers");
    var earlyLogger = new LoggerConfiguration().MinimumLevel.Information().WriteTo.Console();
    Log.Logger = earlyLogger.CreateLogger();

    Log.Information("Setting Up Web App");
    var builder = WebApplication.CreateBuilder(args);

    Log.Information("Setup In Memory Cache");
    builder.Services.AddMemoryCache();

    Log.Information("Setup Azure Key Vault");
    builder.Services.AddAuthenticationInjections();

    EarlyKeyVaultManager keyVaultManager = new EarlyKeyVaultManager(
        builder.Environment.IsProduction()
    );

    Log.Information("Setup Application Insights");
    await builder.SetupApplicationInsights(keyVaultManager);

    Log.Information("Setup Azure Storage Blob");
    await builder.SetupBlobStorage(keyVaultManager);

    Log.Information("Add in Health Checks");
    builder.Services.AddHealthChecks();

    Log.Information("Adding DB Context");
    await builder.AddDatabaseConnection(keyVaultManager, builder.Environment.IsProduction());

    Log.Information("Add Quartz / Cron Scheduler");
    builder.SetupQuartz();

    Log.Information("Setting Up Authentication and Identity");
    builder
        .Services.AddIdentityCore<User>()
        .AddRoles<Role>()
        .AddEntityFrameworkStores<ExpressedRealmsDbContext>()
        .AddApiEndpoints();

    builder.Services.Configure<IdentityOptions>(options =>
    {
        // Default Password settings.
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireUppercase = true;
        options.Password.RequiredLength = 8;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        options.Lockout.AllowedForNewUsers = true;
        options.Lockout.MaxFailedAccessAttempts = 5;
    });

    builder.AddPolicyConfiguration();

    builder.Services.ConfigureHttpJsonOptions(options =>
    {
        options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.SerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    });

    var clientCookieDomain = await keyVaultManager.GetSecret(GeneralConfig.CookieDomain);
    builder
        .Services.AddAuthentication()
        .AddCookie(
            IdentityConstants.BearerScheme,
            o =>
            {
                o.Cookie.Domain = clientCookieDomain;
                o.SlidingExpiration = true;
                o.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                o.Cookie.SameSite = SameSiteMode.None;
            }
        );
    builder.Services.AddAuthorizationBuilder();

    builder.Services.AddAntiforgery(
        (options) =>
        {
            options.Cookie.Domain = clientCookieDomain;
            options.HeaderName = "T-XSRF-TOKEN";
            options.Cookie.HttpOnly = false;
            options.Cookie.Name = "XSRF-TOKEN";
            options.Cookie.SameSite = SameSiteMode.None;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        }
    );

    builder.Services.Configure<ForwardedHeadersOptions>(options =>
    {
        options.ForwardedHeaders =
            ForwardedHeaders.XForwardedFor
            | ForwardedHeaders.XForwardedProto
            | ForwardedHeaders.XForwardedHost;
        options.KnownIPNetworks.Clear();
        options.KnownProxies.Clear();
    });

    Log.Information("Adding OpenAPI Support and Swagger Generation");

    builder.Services.AddOpenApi();

    Log.Information("Configuring various things");
    builder.Services.AddEmailDependencies(builder.Configuration);

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
    await builder.Services.AddFeatureFlagInjections(keyVaultManager);

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

        if (dbContext.Database.GetPendingMigrations().Any())
        {
            Log.Information("DB is missing migrations, running them now");
            dbContext.Database.Migrate();
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
    app.ConfigureUserRoles();

    app.UseDefaultFiles();
    app.UseStaticFiles();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        Log.Information("Setting Up Swagger");
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
    app.Run();
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
