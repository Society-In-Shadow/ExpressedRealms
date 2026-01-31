using Audit.EntityFramework;
using ExpressedRealms.DB.Characters.AssignedXp.AssignedXpMappingModels;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Events.EventScheduleItemsSetup.Audit;
using ExpressedRealms.DB.Models.Events.EventSetup.Audit;
using ExpressedRealms.DB.Models.Events.Questions.EventQuestionSetup;

namespace ExpressedRealms.DB.Models.Events.EventSetup;

[AuditInclude]
public class Event : ISoftDelete
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required DateOnly StartDate { get; set; }
    public required DateOnly EndDate { get; set; }
    public required string Location { get; set; } = null!;
    public required string WebsiteName { get; set; } = null!;
    public required string WebsiteUrl { get; set; } = null!;
    public string? AdditionalNotes { get; set; } = null!;
    public required string TimeZoneId { get; set; } = null!;
    public int ConExperience { get; set; }
    public bool IsPublished { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public virtual ICollection<EventAuditTrail> EventAuditTrails { get; set; } = new HashSet<EventAuditTrail>();
    public virtual ICollection<EventScheduleItemAuditTrail> EventScheduleAuditTrails { get; set; } = new HashSet<EventScheduleItemAuditTrail>();
    public virtual ICollection<AssignedXpMapping> AssignedXpMappings { get; set; } = new HashSet<AssignedXpMapping>();
    public virtual ICollection<EventQuestion> EventQuestions { get; set; } = new HashSet<EventQuestion>();
}
