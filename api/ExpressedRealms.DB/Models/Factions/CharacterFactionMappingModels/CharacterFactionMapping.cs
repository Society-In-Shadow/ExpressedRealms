using Audit.EntityFramework;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Characters;
using ExpressedRealms.DB.Models.Factions.FactionLevelModels;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;

namespace ExpressedRealms.DB.Models.Factions.CharacterFactionMappingModels;

[AuditInclude]
public class CharacterFactionMapping : ISoftDelete
{
    public int Id { get; set; }
    public int CharacterId { get; set; }
    public int FactionLevelId { get; set; }
    public string? ApprovedByUserId { get; set; }
    public string? ApprovalReason { get; set; }
    public string? CharacterNotes { get; set; }
    public bool RequestPromotion { get; set; }
    public string? RequestReason { get; set; }
    public DateTimeOffset ApprovalDate { get; set; }

    public virtual Character Character { get; set; } = null!;
    public virtual FactionLevel FactionLevel { get; set; } = null!;
    public virtual User? ApprovedByUser { get; set; }
    
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}
