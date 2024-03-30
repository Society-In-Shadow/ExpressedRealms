using System.Reflection;
using ExpressedRealms.DB;
using ExpressedRealms.DB.UserProfile.PlayerDBModels;
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

try
{
    Log.Information("Setting Up Web App");
    var builder = WebApplication.CreateBuilder(args);

    Log.Information("Setting Up Loggers");
    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Information()
        .WriteTo.Console()
        .WriteTo.PostgreSQL(
            builder.Configuration.GetConnectionString("DefaultConnection"),
            "Logs",
            needAutoCreateTable: true
        )
        .CreateLogger();

    builder.Host.UseSerilog();

    Log.Information("Adding DB Context");

    builder.Services.AddDbContext<ExpressedRealmsDbContext>(options =>
        options.UseNpgsql(
            builder.Configuration.GetConnectionString("DefaultConnection"),
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
                o.SlidingExpiration = true;
            }
        );
    builder.Services.AddAuthorizationBuilder();

    builder.Services.AddAntiforgery(
        (options) =>
        {
            options.HeaderName = "T-XSRF-TOKEN";
            options.Cookie.HttpOnly = false;
            options.Cookie.Name = "XSRF-TOKEN";
        }
    );

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
    });

    Log.Information("Configuring various things");
    builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
    builder.Services.AddEmailDependencies(builder.Configuration);

    builder.Services.AddValidatorsFromAssemblyContaining<Program>();
    builder.Services.AddFluentValidationAutoValidation();
    builder.Services.AddFluentValidationRulesToSwagger();

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

    app.UseDefaultFiles();
    app.UseStaticFiles();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        Log.Information("Setting Up Swagger");
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    Log.Information("Adding in Security Related Things");
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();

    app.UseAntiforgery();

    Log.Information("Adding endpoints");
    app.AddAuthEndPoints();
    app.AddWeatherEndpoints();
    app.AddCharacterEndPoints();
    app.AddTestingEndPoints();
    app.AddPlayerEndPoints();
    app.AddNavigationEndpoints();
    app.AddExpressionEndpoints();

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
