using Audit.EntityFramework;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Expressions.ExpressionSetup;
using ExpressedRealms.DB.Models.Powers.PowerSetup.Audit;

namespace ExpressedRealms.DB.Models.Powers.PowerPathSetup;

[AuditInclude]
public class PowerPath : ISoftDelete
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    public int ExpressionId { get; set; }
    public virtual Expression Expression { get; set; } = null!;

    public int OrderIndex { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public virtual List<Power> Powers { get; set; } = null!;
    public virtual List<PowerPathAuditTrail> PowerPathAudits { get; set; } = null!;
    public virtual List<PowerAuditTrail> PowerAuditTrails { get; set; } = null!;
}
