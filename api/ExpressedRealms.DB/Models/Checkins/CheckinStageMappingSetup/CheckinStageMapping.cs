using Audit.EntityFramework;
using ExpressedRealms.DB.Models.Checkins.CheckinSetup;
using ExpressedRealms.DB.Models.Checkins.CheckinStageSetup;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;

namespace ExpressedRealms.DB.Models.Checkins.CheckinStageMappingSetup;

[AuditInclude]
public class CheckinStageMapping
{
    public int Id { get; set; }
    public int CheckinId { get; set; }
    public virtual Checkin Checkin { get; set; } = null!;
    public int CheckinStageId { get; set; }
    public virtual CheckinStage CheckinStage { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public required string ApproverUserId { get; set; }
    public virtual User ApproverUser { get; set; } = null!;

}
