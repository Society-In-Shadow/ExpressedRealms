using System.Reflection;
using AspNetCore.SwaggerUI.Themes;
using Azure.Core;
using ExpressedRealms.DB;
using ExpressedRealms.DB.UserProfile.PlayerDBModels;
using ExpressedRealms.Repositories.Characters;
using ExpressedRealms.Repositories.Shared.ExternalDependencies;
using ExpressedRealms.Server.DependencyInjections;
using ExpressedRealms.Server.EndPoints;
using ExpressedRealms.Server.EndPoints.CharacterEndPoints;
using ExpressedRealms.Server.EndPoints.ExpressionEndpoints;
using ExpressedRealms.Server.EndPoints.PlayerEndpoints;
using ExpressedRealms.Server.Swagger;
using FluentValidation;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;
using Swashbuckle.AspNetCore.SwaggerGen;
using Unchase.Swashbuckle.AspNetCore.Extensions.Extensions;
using Azure.Identity;
using Azure.Storage.Blobs;
using ExpressedRealms.Server.Configuration;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;

try
{
    Log.Information("Setting Up Web App");
    var builder = WebApplication.CreateBuilder(args);

    string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "";

    Log.Information("Setting Up Loggers");
    var logger = new LoggerConfiguration()
        .MinimumLevel.Information()
        .WriteTo.Console();

    if (!string.IsNullOrEmpty(connectionString))
    {
        logger.WriteTo.PostgreSQL(
            connectionString,
            "Logs",
            needAutoCreateTable: true
        );
    }
    else
    {
        logger.WriteTo.ApplicationInsights(Environment.GetEnvironmentVariable("APPLICATION_INSIGHTS_CONNECTION_STRING"), TelemetryConverter.Traces);
    }

    Log.Logger = logger.CreateLogger();

    builder.Host.UseSerilog();

    builder.Services.AddApplicationInsightsTelemetry((options) =>
    {
        options.ConnectionString = Environment.GetEnvironmentVariable("APPLICATION_INSIGHTS_CONNECTION_STRING");
    });
    
    // Since we are in a container, we need to keep track of the data keys manually
    var blobStorageEndpoint = Environment.GetEnvironmentVariable("AZURE_STORAGEBLOB_RESOURCEENDPOINT") ?? "";
    if (!string.IsNullOrEmpty(blobStorageEndpoint))
    {
        var blobServiceClient = new BlobServiceClient(new Uri(blobStorageEndpoint), new DefaultAzureCredential());
        var containerClient = blobServiceClient.GetBlobContainerClient("dataprotection-keys");
        var blobClient = containerClient.GetBlobClient("dataprotection-keys.xml");

        builder.Services.AddDataProtection()
            .PersistKeysToAzureBlobStorage(blobClient);
    }

    Log.Information("Add in Healthchecks");

    builder.Services.AddHealthChecks();

    Log.Information("Adding DB Context");

    builder.AddDatabaseConnection(connectionString);

    Log.Information("Setting Up Authentication and Identity");
    builder
        .Services.AddIdentityCore<User>()
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
    });

    builder
        .Services.AddAuthentication()
        .AddCookie(
            IdentityConstants.BearerScheme,
            o =>
            {
                o.Cookie.Domain = Environment.GetEnvironmentVariable("CLIENT_COOKIE_DOMAIN");
                o.SlidingExpiration = true;
                o.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                o.Cookie.SameSite = SameSiteMode.None;
            }
        );
    builder.Services.AddAuthorizationBuilder();

    builder.Services.AddAntiforgery(
        (options) =>
        {
            options.Cookie.Domain = Environment.GetEnvironmentVariable("CLIENT_COOKIE_DOMAIN");
            options.HeaderName = "T-XSRF-TOKEN";
            options.Cookie.HttpOnly = false;
            options.Cookie.Name = "XSRF-TOKEN";
            options.Cookie.SameSite = SameSiteMode.None;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        }
    );

    if (builder.Environment.IsDevelopment())
    {
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins("https://localhost")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
    }

    builder.Services.Configure<ForwardedHeadersOptions>(options =>
    {
        options.ForwardedHeaders =
            ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost;
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

    Log.Information("Building the App");
    var app = builder.Build();

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

    if(app.Environment.IsDevelopment())
    {
        app.UseHttpsRedirection();
        app.UseCors();
    }
    
    app.UseAuthentication();
    app.UseAuthorization();

    app.UseAntiforgery();

    Log.Information("Adding endpoints");
    app.AddAuthEndPoints();
    app.AddCharacterEndPoints();
    app.AddTestingEndPoints();
    app.AddPlayerEndPoints();
    app.AddNavigationEndpoints();
    app.AddExpressionEndpoints();
    app.AddStatEndPoints();

    app.MapFallbackToFile("/index.html");
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
    Log.CloseAndFlush();
}
