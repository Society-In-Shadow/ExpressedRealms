using ExpressedRealms.Characters.Repository.DTOs;
using ExpressedRealms.Characters.Repository.Skills;
using ExpressedRealms.Characters.Repository.Skills.DTOs;
using ExpressedRealms.Characters.Repository.Stats;
using ExpressedRealms.Characters.Repository.Stats.DTOs;
using Microsoft.Extensions.DependencyInjection;

namespace ExpressedRealms.Characters.Repository;

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
