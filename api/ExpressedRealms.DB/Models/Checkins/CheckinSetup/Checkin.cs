using Audit.EntityFramework;
using ExpressedRealms.DB.Models.Checkins.CheckinQuestionResponseSetup;
using ExpressedRealms.DB.Models.Checkins.CheckinQuestionResponseSetup.Audit;
using ExpressedRealms.DB.Models.Checkins.CheckinSetup.Audit;
using ExpressedRealms.DB.Models.Checkins.CheckinStageMappingSetup;
using ExpressedRealms.DB.Models.Events.EventSetup;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.PlayerSetup;

namespace ExpressedRealms.DB.Models.Checkins.CheckinSetup;

[AuditInclude]
public class Checkin
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public virtual Event Event { get; set; } = null!;
    public Guid PlayerId { get; set; }
    public virtual Player Player { get; set; } = null!;

    public virtual ICollection<CheckinAuditTrail> CheckinAuditTrails { get; set; } =
        new HashSet<CheckinAuditTrail>();
    public virtual ICollection<CheckinQuestionResponse> CheckinQuestionResponses { get; set; } =
        new HashSet<CheckinQuestionResponse>();
    public virtual ICollection<CheckinQuestionResponseAuditTrail> CheckinQuestionResponseAuditTrails { get; set; } =
        new HashSet<CheckinQuestionResponseAuditTrail>();
    public virtual ICollection<CheckinStageMapping> CheckinStageMappings { get; set; } =
        new HashSet<CheckinStageMapping>();
}
