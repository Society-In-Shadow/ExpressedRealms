namespace ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMapping;

public interface ICharacterKnowledgeRepository
{
    Task<int> AddCharacterKnowledgeMapping(DB.Models.Knowledges.CharacterKnowledgeMappings.CharacterKnowledgeMapping mapping);
    Task<int> GetExperienceSpentOnKnowledgesForCharacter(int characterId);
    Task<bool> MappingAlreadyExists(int knowledgeId, int characterId);
}