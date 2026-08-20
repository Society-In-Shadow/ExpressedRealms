using System.ComponentModel.DataAnnotations;
using Audit.EntityFramework;
using ExpressedRealms.DB.Models.Characters.CharacterStorage.CharacterStorageModels.Audit;
using ExpressedRealms.DB.Models.Events.EventSetup;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.PlayerSetup;

namespace ExpressedRealms.DB.Models.Characters.CharacterStorage.CharacterStorageModels;

[AuditInclude]
public class CharacterStorageInfo
{
    [Key]
    [Required]
    public int Id { get; set; }
    
    public bool OptedIn { get; set; }
    
    [Required]
    public int EventId { get; set; }

    public virtual Event Event { get; set; } = null!;

    [Required]
    public Guid CollectorUserId { get; set; }

    public virtual Player CollectorUser { get; set; } = null!;
    
    public int Amount { get; set; }
    
    public Guid? SignOffUserId { get; set; }

    public virtual Player? SignOffUser { get; set; } = null!;

    [Required]
    [AuditIgnore]
    public DateTimeOffset Timestamp { get; set; }

    public ICollection<CharacterStorageInfoAuditTrail> CharacterStorageInfoAuditTrails { get; set; }  = new HashSet<CharacterStorageInfoAuditTrail>();
}
