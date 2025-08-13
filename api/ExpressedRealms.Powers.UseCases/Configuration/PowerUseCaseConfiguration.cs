using System.Reflection;
using ExpressedRealms.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace ExpressedRealms.Powers.UseCases.Configuration;

public static class PowerUseCaseConfiguration
{
    public static IServiceCollection AddPowerUseCaseConfiguration(this IServiceCollection services)
    {
        services.ImportGenericUseCases(Assembly.GetExecutingAssembly());
        services.ImportValidators(Assembly.GetExecutingAssembly());

        return services;
    }
}