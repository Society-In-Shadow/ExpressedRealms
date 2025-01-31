using ExpressedRealms.DB;
using ExpressedRealms.DB.Models.Skills;
using ExpressedRealms.Repositories.Characters.Skills.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Repositories.Characters.Skills;

internal sealed class CharacterSkillRepository(
    ExpressedRealmsDbContext context,
    CancellationToken cancellationToken
    ) : ICharacterSkillRepository
{
    public async Task AddDefaultSkills(int characterId)
    {
        var availableSkills = await context.SkillTypes.AsNoTracking()
            .ToListAsync();

        var characterSkills = availableSkills.Select(x => new CharacterSkillsMapping()
        {
            CharacterId = characterId,
            SkillTypeId = x.Id,
            SkillLevelId = 1, // Untrained
        });

        context.CharacterSkillsMappings.AddRange(characterSkills);
        
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<SkillDto>> GetCharacterSkills(int characterId)
    {
        return await context.CharacterSkillsMappings.AsNoTracking()
            .Where(x => x.CharacterId == characterId)
            .Select(x => new SkillDto()
            {
                SkillTypeId = x.SkillTypeId,
                Description = x.SkillType.Description,
                SkillSubTypeId = x.SkillType.SkillSubTypeId,
                Name = x.SkillType.Name,
                LevelId = x.SkillLevelId,
                LevelName = x.SkillLevel.Name,
                LevelDescription = x.SkillType.CharacterLevelDescriptions.First(y => y.SkillLevelId == x.SkillLevelId).Description,
                Benefits = x.SkillType.SkillLevelBenefits
                    .Where(y => y.SkillLevelId == x.SkillLevelId)
                    .Select(y => new BenefitDto()
                    {
                        LevelId = y.SkillLevelId,
                        Name = y.ModifierType.Name,
                        Description = y.ModifierType.Description,
                        Modifier = y.Modifier
                    })
                    .ToList()
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<List<SkillLevelOptionsDto>> GetSkillLevelValuesForSkillTypeId(int skillTypeId)
    {
        var descriptions = await context.SkillLevelDescriptionMappings.AsNoTracking()
            .Where(x => x.SkillLevelId == skillTypeId)
            .Select(x => new SkillLevelOptionsDto()
            {
                SkillTypeId = x.SkillTypeId,
                Name = x.SkillLevel.Name,
                Description = x.Description,
                LevelId = x.SkillLevelId,
                ExperienceCost = x.SkillLevel.XP
            })
            .ToListAsync(cancellationToken);

        var benefits = await context.SkillLevelBenefits.AsNoTracking()
            .Where(x => x.SkillTypeId == skillTypeId)
            .Select(x => new BenefitDto()
            {
                LevelId = x.SkillLevelId,
                Name = x.ModifierType.Name,
                Description = x.ModifierType.Description,
                Modifier = x.Modifier
            }).ToListAsync(cancellationToken);


        foreach (var description in descriptions)
        {
            description.Benefits = benefits.Where(x => x.LevelId == description.LevelId).ToList();
        }

        // TODO: Add overall class that will keep track of current XP expenditure, maybe
        return descriptions;

    }
}