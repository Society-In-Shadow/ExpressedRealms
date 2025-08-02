using ExpressedRealms.DB;
using ExpressedRealms.DB.Models.Knowledges.KnowledgeEducationLevels;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Knowledges.Repository;

public class KnowledgeLevelRepository(
    ExpressedRealmsDbContext context,
    CancellationToken cancellationToken
) : IKnowledgeLevelRepository
{
    public async Task<bool> KnowledgeLevelExists(int id)
    {
        var level = await context.KnowledgeEducationLevels.FirstOrDefaultAsync(x => x.Id == id);
        return level is not null;
    }

    public async Task<KnowledgeEducationLevel> GetKnowledgeLevel(int id)
    {
        return await context.KnowledgeEducationLevels.FirstOrDefaultAsync(x => x.Id == id);
    }
}
