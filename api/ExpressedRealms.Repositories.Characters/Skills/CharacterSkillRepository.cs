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
                Name = x.SkillType.Name,
                Description = x.SkillType.Description,
                LevelId = x.SkillLevelId,
                LevelName = x.SkillLevel.Name
            })
            .ToListAsync(cancellationToken);
    }
}