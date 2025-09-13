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
                Type = x.Blessing.Type,
                BlessingId = x.BlessingId,
                BlessingLevelId = x.BlessingLevelId,
                Id = x.Id,
                Name = x.Blessing.Name,
                Description = x.Blessing.Description,
                LevelName = x.BlessingLevel.Level,
                LevelDescription = x.BlessingLevel.Description,
                Notes = x.Notes,
                SubCategory = x.Blessing.SubCategory,
            })
            .ToListAsync(cancellationToken);
    }

    public Task<int> GetExperienceSpentOnBlessingsForCharacter(int characterId)
    {
        // Is Deleted = false is needed because the UI is filtering out deleted blessings
        return context
            .CharacterBlessingMappings.Where(x =>
                x.CharacterId == characterId && !x.Blessing.IsDeleted
            )
            .SumAsync(x => x.BlessingLevel.XpCost, cancellationToken);
    }

    public Task<int> GetExperienceAvailableToSpendOnCharacter(int characterId)
    {
        // Is Deleted = false is needed because the UI is filtering out deleted blessings
        return context
            .CharacterBlessingMappings.Where(x =>
                x.CharacterId == characterId && !x.Blessing.IsDeleted
            )
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

    public async Task<int> GetSpentXpForBlessingType(int characterId, int blessingId)
    {
        var type = await context
            .Blessings.Where(x => x.Id == blessingId)
            .Select(x => x.Type)
            .FirstAsync();

        // Is Deleted = false is needed because the UI is filtering out deleted blessings
        var xpQuery = context
            .CharacterBlessingMappings.AsNoTracking()
            .Where(x =>
                x.CharacterId == characterId && x.Blessing.Type == type && !x.Blessing.IsDeleted
            );

        if (type.Equals("disadvantage", StringComparison.InvariantCultureIgnoreCase))
        {
            return await xpQuery.SumAsync(x => x.BlessingLevel.XpGain);
        }

        return await xpQuery.SumAsync(x => x.BlessingLevel.XpCost);
    }
}
