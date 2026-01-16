using System.Security.Claims;
using System.Text.Json;
using ExpressedRealms.Admin.API.CustomClaimsPrincipleFactory;
using ExpressedRealms.DB;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.Roles;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;
using ExpressedRealms.Server.Configuration.UserRoles;
using ExpressedRealms.Shared.AzureKeyVault;
using ExpressedRealms.Shared.AzureKeyVault.Secrets;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;

namespace ExpressedRealms.Server.Configuration;

public static class SecurityConfiguration
{
    public static void SetupAuthenticationAndIdentity(WebApplicationBuilder webApplicationBuilder)
    {
        webApplicationBuilder
            .Services.AddIdentityCore<User>()
            .AddRoles<Role>()
            .AddEntityFrameworkStores<ExpressedRealmsDbContext>()
            .AddApiEndpoints();

        webApplicationBuilder.Services.AddScoped<IClaimsTransformation, RedisClaimsTransformer>();
        webApplicationBuilder.Services.AddScoped<ClaimStash>();

        webApplicationBuilder.Services.Configure<IdentityOptions>(options =>
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

        webApplicationBuilder.AddPolicyConfiguration();

        webApplicationBuilder.Services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.SerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
        });

        var clientCookieDomain = KeyVaultManager.GetSecret(GeneralConfig.CookieDomain);
        webApplicationBuilder
            .Services.AddAuthentication()
            .AddCookie(
                IdentityConstants.BearerScheme,
                o =>
                {
                    o.Cookie.Domain = clientCookieDomain;
                    o.SlidingExpiration = true;
                    o.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    o.Cookie.SameSite = SameSiteMode.None;
                    o.Events.OnValidatePrincipal += async (context) =>
                    {
                        var transformer =
                            context.HttpContext.RequestServices.GetRequiredService<IClaimsTransformation>();
                        var principal = await transformer.TransformAsync(context.Principal!);

                        if (principal.Claims.Any(c => c.Type == "KickUserOut"))
                        {
                            context.RejectPrincipal(); // Invalidate the cookie for future requests
                            await context.HttpContext.SignOutAsync(); // Clear cookie on browser on return
                            context.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity()); // treat rest of request as unauthenticated
                        }

                        context.Principal = principal;
                    };
                }
            );
        webApplicationBuilder.Services.AddAuthorizationBuilder();

        webApplicationBuilder.Services.AddAntiforgery(
            (options) =>
            {
                options.Cookie.Domain = clientCookieDomain;
                options.HeaderName = "T-XSRF-TOKEN";
                options.Cookie.HttpOnly = true;
                options.Cookie.Name = "XSRF-TOKEN";
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            }
        );

        webApplicationBuilder.Services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders =
                ForwardedHeaders.XForwardedFor
                | ForwardedHeaders.XForwardedProto
                | ForwardedHeaders.XForwardedHost;
            options.KnownIPNetworks.Clear();
            options.KnownProxies.Clear();
        });
    }
}
