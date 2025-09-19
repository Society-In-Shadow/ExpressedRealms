using ExpressedRealms.Characters.Repository.Xp.Dtos;
using ExpressedRealms.DB;
using ExpressedRealms.DB.Characters.xpTables;
using ExpressedRealms.Repositories.Shared.ExternalDependencies;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Characters.Repository.Xp;

public class XpRepository(
    ExpressedRealmsDbContext context,
    CancellationToken cancellationToken,
    IUserContext userContext
) : IXpRepository
{
    public async Task<CharacterXpView> GetCharacterXpMapping(int characterId, int sectionTypeId)
    {
        return await context.CharacterXpViews.FirstAsync(x =>
            x.CharacterId == characterId && x.SectionTypeId == sectionTypeId
        );
    }

    public async Task AddDefaultCharacterXpMappings(int characterId)
    {
        var sectionTypes = await context
            .XpSectionTypes.AsNoTracking()
            .ToListAsync(cancellationToken);

        context.CharacterXpMappings.AddRange(
            sectionTypes.Select(x => new CharacterXpMapping()
            {
                CharacterId = characterId,
                XpSectionTypeId = x.Id,
                DiscretionXp = -x.SectionCap,
                SpentXp = 0,
                SectionCap = x.SectionCap,
                TotalCharacterCreationXp = 0,
                LevelXp = 0,
            })
        );

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<CharacterXpView>> GetCharacterXpMappings(int characterId)
    {
        return await context
            .CharacterXpViews.AsNoTracking()
            .Where(x => x.CharacterId == characterId)
            .ToListAsync();
    }

    public async Task<int> GetAvailableDiscretionary(int characterId)
    {
        const int maxDiscretionary = 16;

        // get additional discretionary from advantages
        var disadvantageXp = await context
            .CharacterBlessingMappings.AsNoTracking()
            .Where(x => x.CharacterId == characterId && x.Blessing.Type.ToLower() == "disadvantage")
            .SumAsync(x => x.BlessingLevel.XpGain, cancellationToken);

        var availableDiscretionary = maxDiscretionary + disadvantageXp;

        var excludedSections = new List<XpSectionTypes>
        {
            XpSectionTypes.Disadvantages,
            XpSectionTypes.Discretion,
        };
        // If the discretionary is negative, that means the cap isn't reached yet
        var spentXp = await context
            .CharacterXpViews.AsNoTracking()
            .Where(x =>
                x.CharacterId == characterId
                && x.DiscretionXp >= 0
                && !excludedSections.Contains((XpSectionTypes)x.SectionTypeId)
            )
            .SumAsync(x => x.DiscretionXp, cancellationToken);

        return availableDiscretionary - spentXp;
    }

    public async Task<SectionXpDto> GetAvailableXpForSection(
        int characterId,
        XpSectionTypes sectionType
    )
    {
        var characterState = await context
            .Characters.AsNoTracking()
            .Where(x => x.Id == characterId && x.Player.UserId == userContext.CurrentUserId())
            .Select(x => new
            {
                IsPrimaryCharacter = x.IsPrimaryCharacter,
                IsInCharacterCreation = x.IsInCharacterCreation,
                AssignedXp = x.AssignedXp,
            })
            .FirstAsync(cancellationToken);

        var xpInfo = await GetCharacterXpMapping(characterId, (int)sectionType);

        var availableXp = characterState.IsInCharacterCreation switch
        {
            true => await GetAvailableDiscretionary(characterId) + xpInfo.SectionCap,
            false when characterState.IsPrimaryCharacter => xpInfo.TotalCharacterCreationXp
                + characterState.AssignedXp,
            _ => 1000,
        };

        // Discretion is a dyanamic value, as such, it does remove the XP associated with this
        // particular section.  So you need to add it back in to get to the true available XP
        // if the required xp was not fully spent
        if (xpInfo.DiscretionXp > 0)
        {
            availableXp += xpInfo.DiscretionXp;
        }

        return new SectionXpDto() { AvailableXp = availableXp, SpentXp = xpInfo.SpentXp };
    }

    public Task UpdateXpInfo(CharacterXpMapping xpInfo)
    {
        context.CharacterXpMappings.Update(xpInfo);
        return context.SaveChangesAsync();
    }
}
