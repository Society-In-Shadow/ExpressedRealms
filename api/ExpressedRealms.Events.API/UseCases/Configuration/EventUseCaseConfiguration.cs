using System.Reflection;
using ExpressedRealms.Events.API.Repositories.Configuration;
using ExpressedRealms.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace ExpressedRealms.Events.API.UseCases.Configuration;

public static class EventUseCaseConfiguration
{
    public static IServiceCollection AddEventUseCaseInjections(this IServiceCollection services)
    {
        services.ImportGenericUseCases(Assembly.GetExecutingAssembly());
        services.ImportValidators(Assembly.GetExecutingAssembly());
        services.AddEventRepositoryInjections();

        return services;
    }
}
