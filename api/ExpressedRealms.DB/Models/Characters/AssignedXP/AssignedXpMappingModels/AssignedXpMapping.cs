using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Audit.EntityFramework;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Characters.AssignedXP.AssignedXpMappingModels.Audit;
using ExpressedRealms.DB.Models.Characters.AssignedXP.AssignedXpTypeModels;
using ExpressedRealms.DB.Models.Events.EventSetup;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.PlayerSetup;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;

namespace ExpressedRealms.DB.Models.Characters.AssignedXP.AssignedXpMappingModels;

[AuditInclude]
[Table("assigned_xp_mapping")]
public class AssignedXpMapping : ISoftDelete
{
    [Key]
    [Required]
    [Column("id")]
    public int Id { get; set; }

    [Column("character_id")]
    public int? CharacterId { get; set; }

    public virtual Character? Character { get; set; }

    [Required]
    [Column("player_id")]
    public Guid PlayerId { get; set; }

    public virtual Player Player { get; set; } = null!;

    [Required]
    [Column("event_id")]
    public int EventId { get; set; }

    public virtual Event Event { get; set; } = null!;

    [Required]
    [Column("assigned_xp_type_id")]
    public int AssignedXpTypeId { get; set; }

    public virtual AssignedXpType AssignedXpType { get; set; } = null!;

    [Required]
    [Column("assigned_by_user_id")]
    public required string AssignedByUserId { get; set; }

    public virtual User AssignedByUser { get; set; } = null!;

    [MaxLength(1500)]
    [Column("reason")]
    public string? Reason { get; set; }

    [Required]
    [AuditIgnore]
    [Column("timestamp")]
    public DateTimeOffset Timestamp { get; set; }

    [Required]
    [Column("amount")]
    public int Amount { get; set; }

    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public virtual List<AssignedXpMappingAuditTrail> AssignedXpMappingAuditTrails { get; set; } =
        null!;
}
