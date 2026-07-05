using Audit.EntityFramework;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Expressions.ExpressionSetup;
using ExpressedRealms.DB.Models.Powers.PowerPathPowerMappingSetup;

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

    public int? CloneSourceId { get; set; }
    public Guid? CloneBatchId { get; set; }

    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public virtual PowerPath? CloneSource { get; set; }
    public virtual List<PowerPathAuditTrail> PowerPathAudits { get; set; } = null!;
    public virtual ICollection<PowerPathPowerMapping> PowerPathPowerMappings { get; set; } =
        new List<PowerPathPowerMapping>();

    public virtual ICollection<PowerPath> Clones { get; set; } = new List<PowerPath>();
}
