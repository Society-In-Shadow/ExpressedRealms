using ExpressedRealms.Blessings.Repository.CharacterBlessings.dto;
using ExpressedRealms.DB;
using ExpressedRealms.DB.Models.Blessings.CharacterBlessingMappings;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Blessings.Repository.CharacterBlessings;

internal sealed class CharacterBlessingRepository(
    ExpressedRealmsDbContext context,
    CancellationToken cancellationToken
) : ICharacterBlessingRepository
{
    public Task<bool> MappingAlreadyExists(int blessingId, int characterId)
    {
        return context.CharacterBlessingMappings.AnyAsync(
            x => x.BlessingId == blessingId && x.CharacterId == characterId,
            cancellationToken
        );
    }

    public Task<bool> MappingAlreadyExists(int mappingId)
    {
        return context.CharacterBlessingMappings.AnyAsync(
            x => x.Id == mappingId,
            cancellationToken
        );
    }

    public Task<List<CharacterBlessingDto>> GetBlessingsForCharacter(int modelCharacterId)
    {
        return context
            .CharacterBlessingMappings.Where(x => x.CharacterId == modelCharacterId)
            .Select(x => new CharacterBlessingDto()
            {
                BlessingId = x.BlessingId,
                BlessingLevelId = x.BlessingLevelId,
                Id = x.Id,
                Name = x.Blessing.Name,
                Description = x.Blessing.Description,
                LevelName = x.BlessingLevel.Level,
                LevelDescription = x.BlessingLevel.Description,
                Notes = x.Notes,
            })
            .ToListAsync(cancellationToken);
    }

    public Task<int> GetExperienceSpentOnBlessingsForCharacter(int characterId)
    {
        return context
            .CharacterBlessingMappings.Where(x => x.CharacterId == characterId)
            .SumAsync(x => x.BlessingLevel.XpCost, cancellationToken);
    }

    public Task<int> GetExperienceAvailableToSpendOnCharacter(int characterId)
    {
        return context
            .CharacterBlessingMappings.Where(x => x.CharacterId == characterId)
            .SumAsync(x => x.BlessingLevel.XpGain, cancellationToken);
    }

    public Task<CharacterBlessingMapping> GetCharacterBlessingMappingForEditing(int mappingId)
    {
        return context.CharacterBlessingMappings.FirstAsync(
            x => x.Id == mappingId,
            cancellationToken
        );
    }

    public async Task UpdateMapping(CharacterBlessingMapping mapping)
    {
        context.CharacterBlessingMappings.Update(mapping);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> AddCharacterBlessingMapping(
        CharacterBlessingMapping characterBlessingMapping
    )
    {
        context.CharacterBlessingMappings.Add(characterBlessingMapping);
        await context.SaveChangesAsync(cancellationToken);
        return characterBlessingMapping.Id;
    }
}
