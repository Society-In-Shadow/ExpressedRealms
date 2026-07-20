using ExpressedRealms.DB;
using ExpressedRealms.DB.Models.Factions.CharacterFactionMappingModels;
using ExpressedRealms.Expressions.Repository.CharacterFactions.Dtos;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Expressions.Repository.CharacterFactions;

internal sealed class CharacterFactionRepository(
    ExpressedRealmsDbContext context,
    CancellationToken cancellationToken
) : ICharacterFactionRepository
{
    public async Task<int> JoinFaction(CharacterFactionMapping characterFactionMapping)
    {
        context.CharacterFactionMappings.Add(characterFactionMapping);
        await context.SaveChangesAsync(cancellationToken);

        return characterFactionMapping.Id;
    }

    public async Task<List<CharacterFactionDto>> GetLatestPlayerFactionLevels(int characterId)
    {
        return await context
            .CharacterFactionMappings.Where(x => x.CharacterId == characterId)
            .Select(x => new CharacterFactionDto()
            {
                FactionLevelId = x.FactionLevelId,
                Approver = x.ApprovedByUser == null ? "" : x.ApprovedByUser!.Player!.Name,
                ApprovalReason = x.ApprovalReason,
                RequestedPromotion = x.RequestPromotion,
                RequestedPromotionReason = x.RequestReason,
                KnowledgeId = x.FactionLevel.KnowledgeId,
                KnowledgeLevel = x.FactionLevel.KnowledgeLevel,
                KnowledgeSpecialization = x.FactionLevel.Specialization,
                CharacterNotes = x.CharacterNotes,
                ApprovalDate = x.ApprovalDate,
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<List<BasicFactionLevelProjection>> GetFactionLevels(int characterId)
    {
        var factionId = await context
            .CharacterFactionMappings.Where(x => x.CharacterId == characterId)
            .Select(x => new { x.FactionLevel.FactionId })
            .FirstOrDefaultAsync(cancellationToken);

        if (factionId == null)
            return new List<BasicFactionLevelProjection>();

        return await context
            .FactionLevels.Where(x => x.FactionId == factionId.FactionId)
            .Select(x => new BasicFactionLevelProjection()
            {
                Id = x.Id,
                KnowledgeLevel = x.KnowledgeLevel != null ? x.KnowledgeLevel.Level : null,
                Specialization = x.Specialization,
                FactionRankId = x.FactionRankId,
                KnowledgeId = x.KnowledgeId,
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<PlayerFactionInfoDto?> GetPlayerFactionInfo(int characterId)
    {
        return await context
            .CharacterFactionMappings.Where(x => x.CharacterId == characterId)
            .OrderBy(x => x.ApprovalDate)
            .Select(x => new PlayerFactionInfoDto()
            {
                FactionId = x.FactionLevel.FactionId,
                FactionName = x.FactionLevel.Faction.Name,
                FactionLevelId = x.FactionLevelId,
                FactionRank = x.FactionLevel.FactionRank.Name,
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}
