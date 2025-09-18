using ExpressedRealms.DB;
using ExpressedRealms.DB.Characters.xpTables;
using ExpressedRealms.Repositories.Shared.ExternalDependencies;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Characters.Repository.Xp;

public class XpRepository(    
    ExpressedRealmsDbContext context,
    CancellationToken cancellationToken,
    IUserContext userContext) : IXpRepository
{
    public async Task<CharacterXpMapping> GetCharacterXpMapping(int characterId, int sectionTypeId)
    {
        return await context.CharacterXpMappings
            .FirstAsync(x => x.CharacterId == characterId && x.XpSectionTypeId == sectionTypeId);
    }

    public async Task AddDefaultCharacterXpMappings(int characterId)
    {
        var sectionTypes = await context.XpSectionTypes.AsNoTracking().ToListAsync(cancellationToken);
        
        context.CharacterXpMappings.AddRange(sectionTypes.Select(x => new CharacterXpMapping()
        {
            CharacterId = characterId,
            XpSectionTypeId = x.Id,
            DiscretionXp = -x.SectionCap,
            SpentXp = 0,
            SectionCap = x.SectionCap,
            TotalCharacterCreationXp = 0,
            LevelXp = 0,
        }));
        
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<CharacterXpMapping>> GetCharacterXpMappings(int characterId)
    {
        return await context.CharacterXpMappings.AsNoTracking()
            .Include(x => x.XpSectionType)
            .Where(x => x.CharacterId == characterId)
            .ToListAsync();
    }

    public async Task<int> GetAvailableDiscretionary(int characterId)
    {
        const int maxDiscretionary = 16;
        
        // get additional discretionary from advantages
        var disadvantageXp = await context.CharacterBlessingMappings.AsNoTracking()
            .Where(x => x.CharacterId == characterId &&
                        x.Blessing.Type.ToLower() == "disadvantage")
            .SumAsync(x => x.BlessingLevel.XpGain);
        
        var availableDiscretionary = maxDiscretionary + disadvantageXp;
        
        // If the discretionary is negative, that means the cap isn't reached yet
        var spentXp = await context.CharacterXpMappings.AsNoTracking()
            .Where(x => x.CharacterId == characterId && x.DiscretionXp >= 0 && x.XpSectionTypeId != (int)XpSectionTypeEnum.Discretion)
            .SumAsync(x => x.DiscretionXp);
        
        return availableDiscretionary - spentXp;
    }

    public Task UpdateXpInfo(CharacterXpMapping xpInfo)
    {
        context.CharacterXpMappings.Update(xpInfo);
        return context.SaveChangesAsync();
    }
}