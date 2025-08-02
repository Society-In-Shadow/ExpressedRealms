using ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings;
using ExpressedRealms.Knowledges.Repository.Knowledges;
using Microsoft.Extensions.DependencyInjection;

namespace ExpressedRealms.Knowledges.Repository.Configuration;

public static class KnowledgesRepositoryInjections
{
    public static IServiceCollection AddKnowledgeRepositoryInjections(
        this IServiceCollection services
    )
    {
        services.AddScoped<IKnowledgeRepository, KnowledgeRepository>();
        services.AddScoped<ICharacterKnowledgeRepository, CharacterKnowledgeRepository>();
        services.AddScoped<IKnowledgeLevelRepository, KnowledgeLevelRepository>();

        return services;
    }
}
