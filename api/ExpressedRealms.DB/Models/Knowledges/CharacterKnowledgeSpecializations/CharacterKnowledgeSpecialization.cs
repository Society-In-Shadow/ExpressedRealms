using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Knowledges.CharacterKnowledgeMappings;

namespace ExpressedRealms.DB.Models.Knowledges.CharacterKnowledgeSpecializations;

public class CharacterKnowledgeSpecialization : ISoftDelete
{
    public int Id { get; set; }
    public int KnowledgeMappingId { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Notes { get; set; } = null!;

    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public virtual CharacterKnowledgeMapping CharacterKnowledgeMapping { get; set; } = null!;
}
