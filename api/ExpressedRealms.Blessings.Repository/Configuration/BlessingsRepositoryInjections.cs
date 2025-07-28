using ExpressedRealms.Blessings.Repository.Blessings;
using Microsoft.Extensions.DependencyInjection;

namespace ExpressedRealms.Blessings.Repository.Configuration;

public static class BlessingsRepositoryInjections
{
    public static IServiceCollection AddBlessingRepositoryInjections(
        this IServiceCollection services
    )
    {
        services.AddScoped<IBlessingRepository, BlessingRepository>();
        return services;
    }
}
