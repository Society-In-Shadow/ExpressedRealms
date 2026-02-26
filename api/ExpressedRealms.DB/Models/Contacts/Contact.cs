using Audit.EntityFramework;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Characters;
using ExpressedRealms.DB.Models.Contacts.Audit;
using ExpressedRealms.DB.Models.Knowledges.KnowledgeEducationLevels;
using ExpressedRealms.DB.Models.Knowledges.KnowledgeModels;

namespace ExpressedRealms.DB.Models.Contacts;

[AuditInclude]
public class Contact : ISoftDelete
{
    public int Id { get; set; }
    public int CharacterId { get; set; }
    public int KnowledgeId { get; set; }
    public int KnowledgeLevelId { get; set; }
    public required string Name { get; set; }
    public string? Notes { get; set; }
    public byte Frequency { get; set; }
    public byte SpentXp { get; set; }
    public bool IsApproved { get; set; }

    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public virtual Knowledge Knowledge { get; set; } = null!;
    public virtual Character Character { get; set; } = null!;
    public virtual KnowledgeEducationLevel KnowledgeLevel { get; set; } = null!;
    public virtual ICollection<ContactAuditTrail> ContactAuditTrails { get; set; } =
        new List<ContactAuditTrail>();
}
