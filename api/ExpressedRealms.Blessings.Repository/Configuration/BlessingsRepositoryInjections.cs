using ExpressedRealms.Blessings.Repository.Blessings;
using ExpressedRealms.Blessings.Repository.CharacterBlessings;
using Microsoft.Extensions.DependencyInjection;

namespace ExpressedRealms.Blessings.Repository.Configuration;

public static class BlessingsRepositoryInjections
{
    public static IServiceCollection AddBlessingRepositoryInjections(
        this IServiceCollection services
    )
    {
        services.AddScoped<IBlessingRepository, BlessingRepository>();
        services.AddScoped<ICharacterBlessingRepository, CharacterBlessingRepository>();
        return services;
    }
}
