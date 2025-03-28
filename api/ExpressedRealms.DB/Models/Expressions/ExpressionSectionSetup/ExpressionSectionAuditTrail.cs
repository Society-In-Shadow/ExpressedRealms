using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Expressions.ExpressionSetup;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;

namespace ExpressedRealms.DB.Models.Expressions.ExpressionSectionSetup;

public class ExpressionSectionAuditTrail : IAuditTable
{
    public int SectionId { get; set; }
    public int ExpressionId { get; set; }

    public int Id { get; set; }
    public string Action { get; set; }
    public DateTime Timestamp { get; set; }
    public string ActorUserId { get; set; }
    public string ChangedProperties { get; set; }

    public virtual Expression Expression { get; set; }
    public virtual ExpressionSection ExpressionSection { get; set; }
    public virtual User ActorUser { get; set; }
}
