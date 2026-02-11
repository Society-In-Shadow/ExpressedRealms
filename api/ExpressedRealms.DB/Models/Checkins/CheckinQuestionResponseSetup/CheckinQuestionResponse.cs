using System.Text.Json;
using Audit.EntityFramework;
using ExpressedRealms.DB.Models.Checkins.CheckinQuestionResponseSetup.Audit;
using ExpressedRealms.DB.Models.Checkins.CheckinSetup;
using ExpressedRealms.DB.Models.Events.Questions.EventQuestionSetup;

namespace ExpressedRealms.DB.Models.Checkins.CheckinQuestionResponseSetup;

[AuditInclude]
public class CheckinQuestionResponse
{
    public int CheckinId { get; set; }
    public virtual Checkin Checkin { get; set; } = null!;
    public int EventQuestionId { get; set; }
    public virtual EventQuestion EventQuestion { get; set; } = null!;
    public JsonDocument Response { get; set; } = null!;

    public virtual ICollection<CheckinQuestionResponseAuditTrail> CheckinQuestionResponseAuditTrails { get; set; } =
        new HashSet<CheckinQuestionResponseAuditTrail>();
}
