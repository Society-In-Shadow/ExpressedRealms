using ExpressedRealms.DB;
using ExpressedRealms.DB.Models.Knowledges.KnowledgeEducationLevels;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Knowledges.Repository;

internal sealed class KnowledgeLevelRepository(
    ExpressedRealmsDbContext context,
    CancellationToken cancellationToken
) : IKnowledgeLevelRepository
{
    public async Task<bool> KnowledgeLevelExists(int id)
    {
        var level = await context.KnowledgeEducationLevels.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        return level is not null;
    }

    public async Task<KnowledgeEducationLevel> GetKnowledgeLevel(int id)
    {
        return await context.KnowledgeEducationLevels.AsNoTracking()
            .FirstAsync(x => x.Id == id, cancellationToken);
    }
}
