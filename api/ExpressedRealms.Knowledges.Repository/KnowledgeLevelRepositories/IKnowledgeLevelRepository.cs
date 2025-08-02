using ExpressedRealms.DB.Models.Knowledges.KnowledgeEducationLevels;

namespace ExpressedRealms.Knowledges.Repository;

public interface IKnowledgeLevelRepository
{
    Task<bool> KnowledgeLevelExists(int id);
    Task<KnowledgeEducationLevel> GetKnowledgeLevel(int id);
}
