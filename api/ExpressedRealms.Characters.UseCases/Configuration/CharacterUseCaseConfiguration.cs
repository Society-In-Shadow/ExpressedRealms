using System.Reflection;
using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace ExpressedRealms.Characters.UseCases.Configuration;

public static class CharacterUseCaseConfiguration
{
    public static IServiceCollection AddCharacterUseCaseInjections(this IServiceCollection services)
    {
        services.ImportGenericUseCases(Assembly.GetExecutingAssembly());
        services.ImportValidators(Assembly.GetExecutingAssembly());
        services.AddCharacterRepositoryInjections();

        return services;
    }
}
