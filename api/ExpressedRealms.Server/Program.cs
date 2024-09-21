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
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;

try
{
    Log.Information("Setting Up Web App");
    var builder = WebApplication.CreateBuilder(args);
    
    // For system-assigned identity.
    string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "";
    if (string.IsNullOrEmpty(connectionString))
    {
        var sqlServerTokenProvider = new DefaultAzureCredential();
        AccessToken accessToken = await sqlServerTokenProvider.GetTokenAsync(
            new TokenRequestContext(scopes: new string[]
            {
                "https://ossrdbms-aad.database.windows.net/.default"
            }));
    
        connectionString =
            $"{Environment.GetEnvironmentVariable("AZURE_POSTGRESSQL_CONNECTIONSTRING")};Password={accessToken.Token}";
    }

    // Since we are in a container, we need to keep track of the data keys manually
    string blobStorageEndpoint = Environment.GetEnvironmentVariable("azure-storageblob-resourceendpoint-08dee") ?? "";
    if (string.IsNullOrEmpty(blobStorageEndpoint))
    {
        builder.Services.AddDataProtection()
            .PersistKeysToAzureBlobStorage(new Uri(blobStorageEndpoint));
    }

    Log.Information("Setting Up Loggers");
    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Information()
        .WriteTo.Console()
        .WriteTo.PostgreSQL(
            connectionString,
            "Logs",
            needAutoCreateTable: true
        )
        .CreateLogger();

    builder.Host.UseSerilog();
    
    Log.Information("Add in Healthchecks");

    builder.Services.AddHealthChecks();
    
    Log.Information("Adding DB Context");
    
    builder.Services.AddDbContext<ExpressedRealmsDbContext>(options =>
        options.UseNpgsql(connectionString,
            x => x.MigrationsHistoryTable("_EfMigrations", "efcore")
        )
    );

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
