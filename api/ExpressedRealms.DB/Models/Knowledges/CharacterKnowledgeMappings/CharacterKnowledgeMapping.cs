using Audit.EntityFramework;
using ExpressedRealms.DB.Characters;
using ExpressedRealms.DB.Models.Knowledges.KnowledgeEducationLevels;
using ExpressedRealms.DB.Models.Knowledges.KnowledgeModels;

namespace ExpressedRealms.DB.Models.Knowledges.CharacterKnowledgeMappings;

[AuditInclude]
public class CharacterKnowledgeMapping
{
    public int Id { get; set; }
    public int CharacterId { get; set; }
    public int KnowledgeId { get; set; }
    public int KnowledgeLevelId { get; set; }
    public string Notes { get; set; } = null!;

    public virtual Knowledge Knowledge { get; set; } = null!;
    public virtual Character Character { get; set; } = null!;
    public virtual KnowledgeEducationLevel KnowledgeLevel { get; set; } = null!;
    
}
