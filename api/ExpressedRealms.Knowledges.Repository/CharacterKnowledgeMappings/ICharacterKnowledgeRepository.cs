using ExpressedRealms.DB.Models.Knowledges.CharacterKnowledgeMappings;
using ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings.Projections;

namespace ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings;

public interface ICharacterKnowledgeRepository
{
    Task<int> AddCharacterKnowledgeMapping(CharacterKnowledgeMapping mapping);
    Task<int> GetExperienceSpentOnKnowledgesForCharacter(int characterId);
    Task<bool> MappingAlreadyExists(int mappingId);
    Task<bool> MappingAlreadyExists(int knowledgeId, int characterId);
    Task<CharacterKnowledgeMapping> GetCharacterKnowledgeMappingForEditing(int modelMappingId);
    Task UpdateCharacterKnowledgeMapping(CharacterKnowledgeMapping mapping);
    Task<List<CharacterKnowledgeProjection>> GetKnowledgesForCharacter(int characterId);
    Task<SpecializationCountProjection> GetSpecializationCountForMapping(int mappingId);
    Task<bool> HasExistingSpecializationForMapping(int mappingId, string name);
}
