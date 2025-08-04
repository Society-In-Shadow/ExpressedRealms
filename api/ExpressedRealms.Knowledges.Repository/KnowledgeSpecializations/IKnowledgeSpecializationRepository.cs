using ExpressedRealms.DB.Models.Knowledges.CharacterKnowledgeSpecializations;

namespace ExpressedRealms.Knowledges.Repository.KnowledgeSpecializations;

public interface IKnowledgeSpecializationRepository
{
    Task<int> CreateSpecialization(CharacterKnowledgeSpecialization specialization);
}
