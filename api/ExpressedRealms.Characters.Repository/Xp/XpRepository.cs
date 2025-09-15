using ExpressedRealms.DB;
using ExpressedRealms.DB.Characters.xpTables;
using ExpressedRealms.Repositories.Shared.ExternalDependencies;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Characters.Repository.Xp;

public class XpRepository(    
    ExpressedRealmsDbContext context,
    IUserContext userContext) : IXpRepository
{
    public async Task<CharacterXpMapping> GetCharacterXpMapping(int characterId, int sectionTypeId)
    {
        return await context.CharacterXpMappings
            .FirstAsync(x => x.CharacterId == characterId && x.XpSectionTypeId == sectionTypeId);
    }

    public async Task<int> GetAvailableDiscretionary(int characterId)
    {
        const int maxDiscretionary = 16;
        
        // get additional discretionary from advantages
        var disadvantageXp = await context.CharacterBlessingMappings.AsNoTracking()
            .Where(x => x.CharacterId == characterId &&
                        x.Blessing.Type.Equals("disadvantage", StringComparison.InvariantCultureIgnoreCase))
            .SumAsync(x => x.BlessingLevel.XpGain);
        
        var availableDiscretionary = maxDiscretionary + disadvantageXp;
        
        // If the discretionary is negative, that means the cap isn't reached yet
        var spentXp = await context.CharacterXpMappings.AsNoTracking()
            .Where(x => x.CharacterId == characterId && x.DiscretionXp >= 0)
            .SumAsync(x => x.DiscretionXp);
        
        return availableDiscretionary - spentXp;
    }

    public Task UpdateXpInfo(CharacterXpMapping xpInfo)
    {
        context.CharacterXpMappings.Update(xpInfo);
        return context.SaveChangesAsync();
    }
}