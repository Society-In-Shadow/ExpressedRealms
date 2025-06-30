using System.Reflection;
using ExpressedRealms.Powers.Repository.PowerPaths;
using ExpressedRealms.Powers.Repository.PowerPrerequisites;
using ExpressedRealms.Powers.Repository.Powers;
using ExpressedRealms.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace ExpressedRealms.Powers.Repository;

public static class PowersRepositoryInjections
{
    public static IServiceCollection AddPowerRepositoryInjections(this IServiceCollection services)
    {
        services.ImportGenericUseCases(Assembly.GetExecutingAssembly());
        services.ImportValidators(Assembly.GetExecutingAssembly());

        services.AddScoped<IPowerRepository, PowerRepository>();
        services.AddScoped<IPowerPrerequisitesRepository, PowerPrerequisitesRepository>();
        services.AddScoped<IPowerPathRepository, PowerPathRepository>();
        return services;
    }
}
