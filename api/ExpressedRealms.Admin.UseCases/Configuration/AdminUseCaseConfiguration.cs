using System.Reflection;
using ExpressedRealms.Admin.Repository;
using ExpressedRealms.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace ExpressedRealms.Admin.UseCases.Configuration;

public static class AdminUseCaseConfiguration
{
    public static IServiceCollection AddAdminInjections(this IServiceCollection services)
    {
        services.ImportGenericUseCases(Assembly.GetExecutingAssembly());
        services.ImportValidators(Assembly.GetExecutingAssembly());
        services.AddAdminRepositoryInjections();

        return services;
    }
}
