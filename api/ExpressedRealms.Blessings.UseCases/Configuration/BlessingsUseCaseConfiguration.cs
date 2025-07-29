using System.Reflection;
using ExpressedRealms.Blessings.Repository.Configuration;
using ExpressedRealms.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace ExpressedRealms.Blessings.UseCases.Configuration;

public static class BlessingsUseCaseConfiguration
{
    public static IServiceCollection AddBlessingInjections(this IServiceCollection services)
    {
        services.ImportGenericUseCases(Assembly.GetExecutingAssembly());
        services.ImportValidators(Assembly.GetExecutingAssembly());
        services.AddBlessingRepositoryInjections();

        return services;
    }
}
