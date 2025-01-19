using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.UserProfile.PlayerDBModels;

namespace ExpressedRealms.DB.Models.Expressions;

public class ExpressionSectionAuditTrail : IAuditTable
{
    public int SectionId { get; set; }
    public int ExpressionId { get; set; }

    public int Id { get; set; }
    public string Action { get; set; }
    public DateTime Timestamp { get; set; }
    public string UserId { get; set; }
    public string ChangedProperties { get; set; }

    public virtual Expression Expression { get; set; }
    public virtual ExpressionSection ExpressionSection { get; set; }
    public virtual User User { get; set; }
}
