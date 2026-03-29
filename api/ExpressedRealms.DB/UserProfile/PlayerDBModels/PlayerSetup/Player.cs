using Audit.EntityFramework;
using ExpressedRealms.DB.Models.Characters;
using ExpressedRealms.DB.Models.Characters.AssignedXP.AssignedXpMappingModels;
using ExpressedRealms.DB.Models.Checkins.CheckinSetup;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.PlayerAgeGroupSetup;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;

namespace ExpressedRealms.DB.UserProfile.PlayerDBModels.PlayerSetup;

[AuditInclude]
public class Player
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public required string LookupId { get; set; }
    public int? PlayerNumber { get; set; }

    public int? AgeGroupId { get; set; }
    public PlayerAgeGroup? AgeGroup { get; set; }

    public bool HasSignedConsentForm { get; set; } = false;
    public DateTimeOffset? LastAgeGroupCheck { get; set; }

    public bool IsArchetypeAccount { get; set; }
    public bool SendPickupCrbEmail { get; set; } = false;

    public virtual User User { get; set; } = null!;
    public virtual List<Character> Characters { get; set; } = new();
    public virtual List<PlayerAuditTrail> PlayerAuditTrails { get; set; } = new();

    public virtual List<AssignedXpMapping> AssignedXpMappings { get; set; } = null!;
    public virtual ICollection<Checkin> Checkins { get; set; } = new HashSet<Checkin>();
}
