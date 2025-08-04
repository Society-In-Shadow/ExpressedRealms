using ExpressedRealms.DB;
using ExpressedRealms.DB.Models.Knowledges.CharacterKnowledgeSpecializations;

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
}