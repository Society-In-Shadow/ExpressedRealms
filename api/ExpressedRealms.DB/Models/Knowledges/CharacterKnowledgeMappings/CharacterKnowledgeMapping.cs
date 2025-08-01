using Audit.EntityFramework;
using ExpressedRealms.DB.Characters;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Knowledges.CharacterKnowledgeSpecializations;
using ExpressedRealms.DB.Models.Knowledges.KnowledgeEducationLevels;
using ExpressedRealms.DB.Models.Knowledges.KnowledgeModels;

namespace ExpressedRealms.DB.Models.Knowledges.CharacterKnowledgeMappings;

[AuditInclude]
public class CharacterKnowledgeMapping : ISoftDelete
{
    public int Id { get; set; }
    public int CharacterId { get; set; }
    public int KnowledgeId { get; set; }
    public int KnowledgeLevelId { get; set; }
    public string Notes { get; set; } = null!;
    
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public virtual Knowledge Knowledge { get; set; } = null!;
    public virtual Character Character { get; set; } = null!;
    public virtual KnowledgeEducationLevel KnowledgeLevel { get; set; } = null!;
    public virtual List<CharacterKnowledgeSpecialization> CharacterKnowledgeSpecializations { get; set; } = null!;
    
}
