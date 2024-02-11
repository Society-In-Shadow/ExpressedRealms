using System.Security.Claims;
using ExpressedRealms.DB;
using ExpressedRealms.Email.SendGridTestEmail;
using ExpressedRealms.Server.Swagger;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ExpressedRealmsDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
        x => x.MigrationsHistoryTable("_EfMigrations", "efcore")));

builder.Services.AddIdentityCore<IdentityUser>()
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

builder.Services.AddAuthentication().AddCookie(IdentityConstants.BearerScheme, o =>
{
    o.SlidingExpiration = true;
});
builder.Services.AddAuthorizationBuilder();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAntiforgery((options) =>
{
    options.HeaderName = "T-XSRF-TOKEN";
    options.Cookie.HttpOnly = false;
    options.Cookie.Name = "XSRF-TOKEN";
});

builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddEmailDependencies(builder.Configuration);

var app = builder.Build();

// Migrate latest database changes during startup

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<ExpressedRealmsDbContext>();

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

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.MapGet("/characters", [Authorize] async (ExpressedRealmsDbContext dbContext) =>
{
    return await dbContext.Characters.ToListAsync();
})
.WithName("Charcters")
.WithOpenApi()
.RequireAuthorization();

app.MapGroup("auth").MapIdentityApi<IdentityUser>();
app.MapGroup("auth").MapPost("/logoff", (HttpContext httpContext) => Results.SignOut());
app.MapGroup("auth").MapGet("/getAntiforgeryToken", (IAntiforgery _antiforgery, HttpContext httpContext, ClaimsPrincipal user) =>
{
    var tokens = _antiforgery.GetAndStoreTokens(httpContext);
    httpContext.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken,
        new CookieOptions() { HttpOnly = false });
    return Results.Ok();
});
app.MapGet("/sendTestEmail", async (ISendGridEmail email) =>
{
    await email.SendTestEmail();
    return Results.Ok();
});
app.MapFallbackToFile("/index.html");

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
