using System.Reflection;
using ExpressedRealms.Knowledges.Repository.Configuration;
using ExpressedRealms.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace ExpressedRealms.Knowledges.UseCases.Configuration;

public static class KnowledgesUseCaseConfiguration
{
    public static IServiceCollection AddKnowledgesInjections(this IServiceCollection services)
    {
        services.ImportGenericUseCases(Assembly.GetExecutingAssembly());
        services.ImportValidators(Assembly.GetExecutingAssembly());
        services.AddKnowledgeRepositoryInjections();

        return services;
    }
}
