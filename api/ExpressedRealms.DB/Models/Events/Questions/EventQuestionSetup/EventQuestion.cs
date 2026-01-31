using Audit.EntityFramework;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Events.EventSetup;
using ExpressedRealms.DB.Models.Events.Questions.EventQuestionSetup.Audit;
using ExpressedRealms.DB.Models.Events.Questions.QuestionTypeSetup;

namespace ExpressedRealms.DB.Models.Events.Questions.EventQuestionSetup;

[AuditInclude]
public class EventQuestion : ISoftDelete
{
    public int Id { get; set; }
    public required string Question { get; set; }
    public int QuestionTypeId { get; set; }
    public virtual QuestionType QuestionType { get; set; } = null!;
    public int EventId { get; set; }
    public virtual Event Event { get; set; } = null!;

    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public virtual ICollection<EventQuestionAuditTrail> EventQuestionAuditTrails { get; set; } =
        new HashSet<EventQuestionAuditTrail>();
}
