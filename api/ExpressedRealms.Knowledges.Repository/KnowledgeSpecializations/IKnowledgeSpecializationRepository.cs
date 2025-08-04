using ExpressedRealms.DB.Models.Knowledges.CharacterKnowledgeSpecializations;

namespace ExpressedRealms.Knowledges.Repository.KnowledgeSpecializations;

public interface IKnowledgeSpecializationRepository
{
    Task<int> CreateSpecialization(CharacterKnowledgeSpecialization specialization);
    Task<bool> SpecializationExists(int id);
    Task<CharacterKnowledgeSpecialization> GetSpecialization(int id);
    Task UpdateSpecialization(CharacterKnowledgeSpecialization specialization);
}
