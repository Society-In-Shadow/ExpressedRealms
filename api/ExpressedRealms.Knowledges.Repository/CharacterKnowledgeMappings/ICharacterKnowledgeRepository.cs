using ExpressedRealms.DB.Models.Knowledges.CharacterKnowledgeMappings;

namespace ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings;

public interface ICharacterKnowledgeRepository
{
    Task<int> AddCharacterKnowledgeMapping(CharacterKnowledgeMapping mapping);
    Task<int> GetExperienceSpentOnKnowledgesForCharacter(int characterId);
    Task<bool> MappingAlreadyExists(int knowledgeId, int characterId);
}
