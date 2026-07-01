using ExpressedRealms.DB;
using ExpressedRealms.DB.Models.Factions.FactionLevelModels;
using ExpressedRealms.Expressions.Repository.Factions.Dtos;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Expressions.Repository.FactionLevels;

internal sealed class FactionLevelRepository(
    ExpressedRealmsDbContext context,
    CancellationToken cancellationToken
) : IFactionLevelRepository
{
    public async Task CreateFactionLevelsAsync(List<FactionLevel> factionLevels)
    {
        context.FactionLevels.AddRange(factionLevels);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task EditFactionLevelAsync(FactionLevel factionlevel)
    {
        context.FactionLevels.Update(factionlevel);
        await context.SaveChangesAsync(cancellationToken);
    }

    public Task<FactionLevel?> GetFactionLevelForEditingAsync(int id)
    {
        return context.FactionLevels.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<List<FactionLevelListDto>> GetFactionLevels()
    {
        return await context
            .FactionLevels.AsNoTracking()
            .Select(x => new FactionLevelListDto()
            {
                Id = x.Id,
                RankName = x.FactionRank.Name,
                KnowledgeId = x.KnowledgeId,
                Knowledge = x.KnowledgeId == null ? null : x.Knowledge!.Name,
                KnowledgeLevel = x.KnowledgeLevelId == null ? null : x.KnowledgeLevel!.Name,
                KnowledgeLevelId = x.KnowledgeLevelId,
                Specialization = x.Specialization,
            })
            .ToListAsync(cancellationToken);
    }

    public Task<FactionLevel?> GetFactionLevelAsync(int id)
    {
        return context
            .FactionLevels.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}
