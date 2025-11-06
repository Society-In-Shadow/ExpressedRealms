using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Audit.EntityFramework;
using ExpressedRealms.DB.Characters.AssignedXp.AssignedXpMappingModels;
using ExpressedRealms.DB.Characters.AssignedXP.AssignedXpTypeModels.Audit;
using ExpressedRealms.DB.Interceptors;

namespace ExpressedRealms.DB.Characters.AssignedXp.AssignedXpTypeModels;

[AuditInclude]
[Table("assigned_xp_type")]
public class AssignedXpType : ISoftDelete
{
    [Key]
    [Required]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(300)]
    public required string Name { get; set; }

    [MaxLength(1500)]
    public string? Description { get; set; }

    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public virtual List<AssignedXpTypeAuditTrail> AssignedXpTypeAuditTrails { get; set; } = null!;
    public virtual List<AssignedXpMapping> AssignedXpMappings { get; set; } = null!;
}
