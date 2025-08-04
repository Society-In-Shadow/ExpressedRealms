using ExpressedRealms.DB;
using ExpressedRealms.DB.Models.Knowledges.CharacterKnowledgeSpecializations;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Knowledges.Repository.KnowledgeSpecializations;

public class KnowledgeSpecializationRepository(
    ExpressedRealmsDbContext context,
    CancellationToken cancellationToken
) : IKnowledgeSpecializationRepository
{
    public async Task<int> CreateSpecialization(CharacterKnowledgeSpecialization specialization)
    {
        context.CharacterKnowledgeSpecializations.Add(specialization);
        await context.SaveChangesAsync(cancellationToken);
        return specialization.Id;
    }

    public async Task<bool> SpecializationExists(int id)
    {
        return await context.CharacterKnowledgeSpecializations.AnyAsync(x => x.Id == id);
    }

    public async Task<CharacterKnowledgeSpecialization> GetSpecialization(int id)
    {
        return await context.CharacterKnowledgeSpecializations.FirstAsync(x => x.Id == id);
    }

    public async Task UpdateSpecialization(CharacterKnowledgeSpecialization specialization)
    {
        context.Update(specialization);
        await context.SaveChangesAsync(cancellationToken);
    }
}
