using Audit.EntityFramework;
using ExpressedRealms.DB.Models.Characters;
using ExpressedRealms.DB.Models.Factions.FactionRankModels;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;

namespace ExpressedRealms.DB.Models.Factions.CharacterFactionMappingModels;

[AuditInclude]
public class CharacterFactionMapping
{
    public int Id { get; set; }
    public int CharacterId { get; set; }
    public int FactionRankId { get; set; }
    public string? ApprovedByUserId { get; set; }
    public string? ApprovalReason { get; set; }
    public string? CharacterNotes { get; set; }
    public bool RequestPromotion { get; set; }
    public string? RequestReason { get; set; }
    public DateTimeOffset ApprovalDate { get; set; }

    public virtual Character Character { get; set; } = null!;
    public virtual FactionRank FactionRank { get; set; } = null!;
    public virtual User? ApprovedByUser { get; set; }
}
