using Audit.EntityFramework;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Creatures.CreatureSetup.Audit;
using ExpressedRealms.DB.Models.Creatures.PublishType;

namespace ExpressedRealms.DB.Models.Creatures.CreatureSetup;

[AuditInclude]
public class Creature : ISoftDelete
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    
    public int CreaturePublishTypeId { get; set; }
    public virtual CreaturePublishType CreaturePublishType { get; set; } = null!;
    
    
    public bool IsPublished { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public virtual ICollection<CreatureAuditTrail> CreatureAuditTrails { get; set; } =
        new HashSet<CreatureAuditTrail>();

}
