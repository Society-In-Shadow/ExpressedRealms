using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;

namespace ExpressedRealms.DB.Models.Characters.CharacterStorage.CharacterStorageModels.Audit;

public class CharacterStorageInfoAuditTrail : IAuditTable
{
    public int CharacterStorageInfoId { get; set; }

    public virtual CharacterStorageInfo CharacterStorageInfo { get; set; } = null!;

    public int Id { get; set; }
    public required string Action { get; set; }
    public DateTime Timestamp { get; set; }
    public required string ActorUserId { get; set; }
    public required string ChangedProperties { get; set; }
    public virtual User ActorUser { get; set; } = null!;
}
