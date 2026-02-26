using System.ComponentModel.DataAnnotations;
using Audit.EntityFramework;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Characters.AssignedXP.AssignedXpMappingModels.Audit;
using ExpressedRealms.DB.Models.Characters.AssignedXP.AssignedXpTypeModels;
using ExpressedRealms.DB.Models.Events.EventSetup;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.PlayerSetup;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;

namespace ExpressedRealms.DB.Models.Characters.AssignedXP.AssignedXpMappingModels;

[AuditInclude]
public class AssignedXpMapping : ISoftDelete
{
    [Key]
    [Required]
    public int Id { get; set; }

    public int? CharacterId { get; set; }

    public virtual Character? Character { get; set; }

    [Required]
    public Guid PlayerId { get; set; }

    public virtual Player Player { get; set; } = null!;

    [Required]
    public int EventId { get; set; }

    public virtual Event Event { get; set; } = null!;

    [Required]
    public int AssignedXpTypeId { get; set; }

    public virtual AssignedXpType AssignedXpType { get; set; } = null!;

    [Required]
    public required string AssignedByUserId { get; set; }

    public virtual User AssignedByUser { get; set; } = null!;

    [MaxLength(1500)]
    public string? Reason { get; set; }

    [Required]
    [AuditIgnore]
    public DateTimeOffset Timestamp { get; set; }

    [Required]
    public int Amount { get; set; }

    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public virtual List<AssignedXpMappingAuditTrail> AssignedXpMappingAuditTrails { get; set; } =
        null!;
}
