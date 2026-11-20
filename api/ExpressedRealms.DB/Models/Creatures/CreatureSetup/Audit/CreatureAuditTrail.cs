using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;

namespace ExpressedRealms.DB.Models.Creatures.CreatureSetup.Audit;

public class CreatureAuditTrail : IAuditTable
{
    public int CreatureId { get; set; }
    public virtual Creature Creature { get; set; } = null!;

    public int Id { get; set; }
    public required string Action { get; set; }
    public DateTime Timestamp { get; set; }
    public required string ActorUserId { get; set; }
    public required string ChangedProperties { get; set; }
    public virtual User ActorUser { get; set; } = null!;
}
