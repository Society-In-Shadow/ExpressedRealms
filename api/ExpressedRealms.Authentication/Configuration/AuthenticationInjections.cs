using ExpressedRealms.Authentication.AzureKeyVault;
using ExpressedRealms.Authentication.PermissionCollection.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace ExpressedRealms.Authentication.Configuration;

public static class AuthenticationInjections
{
    public static IServiceCollection AddAuthenticationInjections(this IServiceCollection services)
    {
        services.AddSingleton<IKeyVaultManager, KeyVaultManager>();
        services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
        return services;
    }
}
