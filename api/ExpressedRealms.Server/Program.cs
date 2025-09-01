using System.Reflection;
using AspNetCore.Swagger.Themes;
using Audit.Core;
using ExpressedRealms.Authentication.AzureKeyVault;
using ExpressedRealms.Authentication.AzureKeyVault.Secrets;
using ExpressedRealms.Authentication.Configuration;
using ExpressedRealms.Blessings.API.Configuration;
using ExpressedRealms.Blessings.UseCases.Configuration;
using ExpressedRealms.Characters.API.Configuration;
using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.UseCases.Configuration;
using ExpressedRealms.DB;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.Roles;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;
using ExpressedRealms.Email;
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
using ExpressedRealms.Repositories.Admin;
using ExpressedRealms.Repositories.Shared.ExternalDependencies;
using ExpressedRealms.Server.Configuration;
using ExpressedRealms.Server.Configuration.UserRoles;
using ExpressedRealms.Server.DependencyInjections;
using ExpressedRealms.Server.EndPoints;
using ExpressedRealms.Server.EndPoints.AdminEndpoints;
using ExpressedRealms.Server.EndPoints.PlayerEndpoints;
using ExpressedRealms.Server.Shared.Extensions;
using ExpressedRealms.Server.Swagger;
using FluentValidation;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;
using Swashbuckle.AspNetCore.SwaggerGen;
using Unchase.Swashbuckle.AspNetCore.Extensions.Extensions;

try
{
    var earlyLogger = new LoggerConfiguration().MinimumLevel.Information().WriteTo.Console();
    Log.Logger = earlyLogger.CreateLogger();

    Log.Information("Setting Up Loggers");
    var logger = new LoggerConfiguration().MinimumLevel.Information().WriteTo.Console();

    Log.Information("Setting Up Web App");
    var builder = WebApplication.CreateBuilder(args);

    Log.Information("Setup In Memory Cache");

    builder.Services.AddMemoryCache();

    Log.Information("Setup Azure Key Vault");

    builder.Services.AddAuthenticationInjections();

    EarlyKeyVaultManager keyVaultManager = new EarlyKeyVaultManager(
        builder.Environment.IsProduction()
    );

    if (builder.Environment.IsDevelopment())
    {
        string connectionString = await keyVaultManager.GetSecret(ConnectionStrings.Database);
        logger.WriteTo.PostgreSQL(connectionString, "Logs", needAutoCreateTable: true);
    }
    else
    {
        var appInsightsConnectionString = await keyVaultManager.GetSecret(
            ConnectionStrings.ApplicationInsights
        );
        logger.WriteTo.ApplicationInsights(appInsightsConnectionString, TelemetryConverter.Traces);
        builder.Services.AddApplicationInsightsTelemetry(
            (options) =>
            {
                options.ConnectionString = appInsightsConnectionString;
            }
        );
    }

    Log.Logger = logger.CreateLogger();

    builder.Host.UseSerilog();

    Log.Information("Setup Azure Storage Blob");

    await builder.SetupBlobStorage(keyVaultManager);

    Log.Information("Add in Healthchecks");

    builder.Services.AddHealthChecks();

    Log.Information("Adding DB Context");

    await builder.AddDatabaseConnection(keyVaultManager, builder.Environment.IsProduction());

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
        options.KnownNetworks.Clear();
        options.KnownProxies.Clear();
    });

    Log.Information("Adding OpenAPI Support and Swagger Generation");

    // Add services to the container.
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        // Needed to add additional information on dto's
        // https://github.com/domaindrivendev/Swashbuckle.AspNetCore?tab=readme-ov-file#include-descriptions-from-xml-comments
        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        options.AddEnumsWithValuesFixFilters();
    });

    Log.Information("Configuring various things");
    builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
    builder.Services.AddEmailDependencies(builder.Configuration);

    builder.Services.AddValidatorsFromAssemblyContaining<Program>();
    builder.Services.AddFluentValidationAutoValidation();
    builder.Services.AddFluentValidationRulesToSwagger();

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
    builder.Services.AddAdminRepositoryInjections();
    builder.Services.AddPowerRepositoryInjections();
    builder.Services.AddKnowledgesInjections();
    builder.Services.AddExpressionTextSectionInjections();
    builder.Services.AddBlessingInjections();
    builder.Services.AddPowerUseCaseConfiguration();
    builder.Services.AddCharacterUseCaseInjections();
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

    Log.Information("Updating Feature Flags");
    using (var scope = app.Services.CreateScope())
    {
        var featureToggleManager =
            scope.ServiceProvider.GetRequiredService<IFeatureToggleManager>();
        await featureToggleManager.UpdateFeatureToggles();
    }

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
        app.UseSwagger();
        app.UseSwaggerUI(ModernStyle.Dark);
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
    app.AddAdminEndPoints();
    app.AddPowerEndPoints();
    app.AddCharacterApiEndPoints();
    app.ConfigureBlessingEndPoints();
    app.ConfigureKnowledgeEndPoints();

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
