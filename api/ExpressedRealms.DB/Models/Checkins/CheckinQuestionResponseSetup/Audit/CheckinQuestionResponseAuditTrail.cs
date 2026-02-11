using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Checkins.CheckinSetup;
using ExpressedRealms.DB.Models.Events.Questions.EventQuestionSetup;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;

namespace ExpressedRealms.DB.Models.Checkins.CheckinQuestionResponseSetup.Audit;

public class CheckinQuestionResponseAuditTrail : IAuditTable
{
    public virtual CheckinQuestionResponse CheckinQuestionResponse { get; set; } = null!;

    public int CheckinId { get; set; }
    public virtual Checkin Checkin { get; set; } = null!;
    public int EventQuestionId { get; set; }
    public virtual EventQuestion EventQuestion { get; set; } = null!;

    public int Id { get; set; }
    public required string Action { get; set; }
    public DateTime Timestamp { get; set; }
    public required string ActorUserId { get; set; }
    public required string ChangedProperties { get; set; }
    public virtual User ActorUser { get; set; } = null!;
}
