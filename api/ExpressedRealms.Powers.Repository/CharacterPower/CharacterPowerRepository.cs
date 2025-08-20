using ExpressedRealms.DB;
using ExpressedRealms.DB.Models.Powers.CharacterPowerMappingSetup;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Powers.Repository.CharacterPower;

internal sealed class CharacterPowerRepository(
    ExpressedRealmsDbContext context,
    CancellationToken token
) : ICharacterPowerRepository
{
    public async Task<bool> MappingExistsAsync(int powerId, int characterId)
    {
        return await context.CharacterPowerMappings.AnyAsync(
            x => x.PowerId == powerId && x.CharacterId == characterId,
            token
        );
    }

    public async Task<int> GetExperienceSpentOnPowersForCharacter(int modelCharacterId)
    {
        return await context.CharacterPowerMappings.SumAsync(x => x.PowerLevel.Xp, token);
    }

    public async Task<int> AddCharacterPowerMapping(CharacterPowerMapping characterPowerMapping)
    {
        context.CharacterPowerMappings.Add(characterPowerMapping);
        await context.SaveChangesAsync(token);
        return characterPowerMapping.Id;
    }

    public async Task<List<int>> GetSelectablePowersForCharacter(int characterId)
    {
        var expressionId = await context
            .Characters.Where(x => x.Id == characterId)
            .Select(x => x.ExpressionId)
            .FirstAsync(token);

        // Get all the assigned powers
        var powerMappings = await context
            .CharacterPowerMappings.Where(x => x.CharacterId == characterId)
            .Select(x => x.PowerId)
            .ToListAsync(token);

        // Grab all powers plus prerequisite data
        var allPowers = await context
            .Powers.Where(x => x.PowerPath.ExpressionId == expressionId)
            .Select(x => new
            {
                PowerId = x.Id,
                RequiredAmount = x.Prerequisite != null ? x.Prerequisite.RequiredAmount : 0,
                PrerequisitePowers = x.Prerequisite != null
                    ? x.Prerequisite.PrerequisitePowers.Select(y => y.PowerId)
                    : null,
            })
            .ToListAsync();

        // Get all powers whose requirements have been fulfilled
        var selectablePowers = allPowers
            .Where(x =>
                // If a power does not have a prerequisite, it is always selectable
                x.PrerequisitePowers == null
                ||
                // Otherwise, if the assigned power mappings have everything needed for a power, it is also selectable
                x.PrerequisitePowers.Intersect(powerMappings).Count() >= x.RequiredAmount
            )
            .Select(x => x.PowerId)
            .ToList();

        // Already selected powers should not be added again
        return selectablePowers.Except(powerMappings).ToList();
    }
}
