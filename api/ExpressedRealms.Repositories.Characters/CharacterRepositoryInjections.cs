using ExpressedRealms.Repositories.Characters.DTOs;
using ExpressedRealms.Repositories.Characters.Skills;
using ExpressedRealms.Repositories.Characters.Skills.DTOs;
using ExpressedRealms.Repositories.Characters.Stats;
using ExpressedRealms.Repositories.Characters.Stats.DTOs;
using Microsoft.Extensions.DependencyInjection;

namespace ExpressedRealms.Repositories.Characters;

public static class CharacterRepositoryInjections
{
    public static IServiceCollection AddCharacterRepositoryInjections(
        this IServiceCollection services
    )
    {
        services.AddScoped<AddCharacterDtoValidator>();
        services.AddScoped<EditCharacterDtoValidator>();
        services.AddScoped<EditCharacterSkillMappingDtoValidator>();
        services.AddScoped<ICharacterRepository, CharacterRepository>();

        services.AddScoped<EditStatDtoValidator>();
        services.AddScoped<GetDetailedStatInfoDtoValidator>();
        services.AddScoped<ICharacterStatRepository, CharacterStatRepository>();
        services.AddScoped<ICharacterSkillRepository, CharacterSkillRepository>();

        return services;
    }
}
