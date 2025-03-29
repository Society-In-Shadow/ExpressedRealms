using ExpressedRealms.Repositories.Powers.Powers;
using Microsoft.Extensions.DependencyInjection;

namespace ExpressedRealms.Repositories.Powers;

public static class PowersRepositoryInjections
{
    public static IServiceCollection AddPowerRepositoryInjections(this IServiceCollection services)
    {
        services.AddScoped<IPowerRepository, PowerRepository>();
        return services;
    }
}
