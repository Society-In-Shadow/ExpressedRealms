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
        return await context.CharacterPowerMappings
            .AnyAsync(x => x.PowerId == powerId && x.CharacterId == characterId,
            token);
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
}