using ExpressedRealms.DB;
using ExpressedRealms.DB.Models.Knowledges;
using ExpressedRealms.DB.Models.Knowledges.KnowledgeModels;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Knowledges.Repository.Knowledges;

internal sealed class KnowledgeRepository(
    ExpressedRealmsDbContext context,
    CancellationToken cancellationToken
) : IKnowledgeRepository
{
    public async Task<int> CreateKnowledgeAsync(Knowledge knowledge)
    {
        context.Knowledges.Add(knowledge);
        await context.SaveChangesAsync(cancellationToken);
        return knowledge.Id;
    }

    public async Task<bool> HasDuplicateName(string name, int knowledgeId = 0)
    {
        if (knowledgeId != 0)
        {
            return await context
                .Knowledges.AsNoTracking()
                .AnyAsync(
                    x => x.Name.ToLower() == name.ToLower() && x.Id != knowledgeId,
                    cancellationToken
                );
        }
        return await context
            .Knowledges.AsNoTracking()
            .AnyAsync(x => x.Name.ToLower() == name.ToLower(), cancellationToken);
    }

    public async Task<bool> KnowledgeTypeExists(int knowledgeTypeId)
    {
        return await context
            .KnowledgeTypes.AsNoTracking()
            .AnyAsync(x => x.Id == knowledgeTypeId, cancellationToken);
    }

    public async Task<bool> IsExistingKnowledge(int knowledgeId)
    {
        return await context
            .Knowledges.AsNoTracking()
            .AnyAsync(x => x.Id == knowledgeId, cancellationToken);
    }

    public async Task EditKnowledgeAsync(Knowledge knowledge)
    {
        context.Knowledges.Update(knowledge);
        await context.SaveChangesAsync(cancellationToken);
    }

    public Task<Knowledge> GetKnowledgeForEditingAsync(int modelId)
    {
        return context.Knowledges.FirstAsync(x => x.Id == modelId, cancellationToken);
    }

    public async Task<List<Knowledge>> GetKnowledges()
    {
        return await context
            .Knowledges.Include(x => x.KnowledgeType)
            .ToListAsync(cancellationToken);
    }

    public Task<Knowledge> GetKnowledgeAsync(int modelId)
    {
        return context
            .Knowledges.AsNoTracking()
            .FirstAsync(x => x.Id == modelId, cancellationToken);
    }

    public Task<List<KnowledgeType>> GetKnowledgeTypesAsync()
    {
        return context.KnowledgeTypes.AsNoTracking().ToListAsync(cancellationToken);
    }
}
