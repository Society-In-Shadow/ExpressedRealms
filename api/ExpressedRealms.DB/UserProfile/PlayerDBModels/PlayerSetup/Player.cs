using Audit.EntityFramework;
using ExpressedRealms.DB.Characters;
using ExpressedRealms.DB.Characters.AssignedXp.AssignedXpMappingModels;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;

namespace ExpressedRealms.DB.UserProfile.PlayerDBModels.PlayerSetup;

[AuditInclude]
public class Player
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public required string LookupId { get; set; }

    public virtual User User { get; set; } = null!;
    public virtual List<Character> Characters { get; set; } = new();
    public virtual List<PlayerAuditTrail> PlayerAuditTrails { get; set; } = new();

    public virtual List<AssignedXpMapping> AssignedXpMappings { get; set; } = null!;
}
