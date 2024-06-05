using Microsoft.Extensions.DependencyInjection;

namespace ExpressedRealms.Repositories.Characters;

public static class CharacterRepositoryInjections
{
    public static IServiceCollection AddCharacterRepositoryInjections(
        this IServiceCollection services
    )
    {
        services.AddScoped<ICharacterRepository, CharacterRepository>();
        return services;
    }
}
