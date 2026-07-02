using ExpressedRealms.DB;
using ExpressedRealms.DB.Models.Factions.FactionModels;
using ExpressedRealms.Expressions.Repository.Factions.Dtos;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Expressions.Repository.Factions;

internal sealed class FactionRepository(
    ExpressedRealmsDbContext context,
    CancellationToken cancellationToken
) : IFactionRepository
{
    public async Task<int> CreateFactionAsync(Faction faction)
    {
        context.Factions.Add(faction);
        await context.SaveChangesAsync(cancellationToken);
        return faction.Id;
    }

    public async Task<bool> HasDuplicateName(string name, int factionId = 0)
    {
        if (factionId != 0)
        {
            return await context
                .Factions.AsNoTracking()
                .AnyAsync(
                    x => x.Name.ToLower() == name.ToLower() && x.Id != factionId,
                    cancellationToken
                );
        }
        return await context
            .Factions.AsNoTracking()
            .AnyAsync(x => x.Name.ToLower() == name.ToLower(), cancellationToken);
    }

    public async Task EditFactionAsync(Faction faction)
    {
        context.Factions.Update(faction);
        await context.SaveChangesAsync(cancellationToken);
    }

    public Task<Faction?> GetFactionForEditingAsync(int id)
    {
        return context.Factions.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<List<FactionDto>> GetFactions(int expressionId)
    {
        return await context
            .Factions.AsNoTracking()
            .OrderBy(x => x.Name)
            .Select(x => new FactionDto()
            {
                Id = x.Id,
                Name = x.Name,
                Background = x.Background,
                ExpressionId = x.ExpressionId,
                Levels = x
                    .FactionLevels.Select(y => new FactionLevelListDto()
                    {
                        Id = y.Id,
                        RankName = y.FactionRank.Name,
                        KnowledgeId = y.KnowledgeId,
                        Knowledge = y.KnowledgeId == null ? null : y.Knowledge!.Name,
                        KnowledgeLevel =
                            y.KnowledgeLevelId == null
                                ? null
                                : $"{y.KnowledgeLevel!.Name} ({y.KnowledgeLevel.Level})",
                        KnowledgeLevelId = y.KnowledgeLevelId,
                        Specialization = y.Specialization,
                        PowerId = y.PowerId
                    })
                    .ToList(),
            })
            .Where(x => x.ExpressionId == expressionId)
            .ToListAsync(cancellationToken);
    }

    public Task<Faction?> GetFactionAsync(int id)
    {
        return context
            .Factions.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}
