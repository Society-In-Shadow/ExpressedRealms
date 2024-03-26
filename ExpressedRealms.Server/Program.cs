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
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ExpressedRealmsDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        x => x.MigrationsHistoryTable("_EfMigrations", "efcore")
    )
);

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
builder.Services.AddAntiforgery(
    (options) =>
    {
        options.HeaderName = "T-XSRF-TOKEN";
        options.Cookie.HttpOnly = false;
        options.Cookie.Name = "XSRF-TOKEN";
    }
);

builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddEmailDependencies(builder.Configuration);

builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationRulesToSwagger();

var app = builder.Build();

// Migrate latest database changes during startup

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ExpressedRealmsDbContext>();

    if (dbContext.Database.GetPendingMigrations().Any())
    {
        dbContext.Database.Migrate();
    }
}

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.AddAuthEndPoints();
app.AddWeatherEndpoints();
app.AddCharacterEndPoints();
app.AddTestingEndPoints();
app.AddPlayerEndPoints();
app.AddNavigationEndpoints();
app.AddExpressionEndpoints();

app.MapFallbackToFile("/index.html");

app.Run();
