using ExpressedRealms.DB.Models.Knowledges;
using ExpressedRealms.DB.Models.Knowledges.KnowledgeModels;

namespace ExpressedRealms.Knowledges.Repository.Knowledges;

public interface IKnowledgeRepository
{
    Task<int> CreateKnowledgeAsync(Knowledge knowledge);
    Task<bool> HasDuplicateName(string name);
    Task<bool> KnowledgeTypeExists(int knowledgeTypeId);
    Task<bool> IsExistingKnowledge(int knowledgeId);
    Task EditKnowledgeAsync(Knowledge knowledge);
    Task<Knowledge> GetKnowledgeForEditingAsync(int modelId);
    Task<List<Knowledge>> GetKnowledges();
    Task<Knowledge> GetKnowledgeAsync(int modelId);
    Task<List<KnowledgeType>> GetKnowledgeTypesAsync();
}
