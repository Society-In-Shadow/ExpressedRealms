using ExpressedRealms.Authentication.PermissionCollection.Configuration;
using ExpressedRealms.Authentication.PermissionCollection.PermissionManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace ExpressedRealms.Authentication.Configuration;

public static class AuthenticationInjections
{
    public static IServiceCollection AddAuthenticationInjections(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
        services.AddScoped<IPermissionManager, PermissionManager>();
        return services;
    }
}
