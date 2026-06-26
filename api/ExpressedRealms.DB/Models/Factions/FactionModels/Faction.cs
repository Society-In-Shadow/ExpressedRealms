using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Contacts;
using ExpressedRealms.DB.Models.Expressions.ExpressionSetup;
using ExpressedRealms.DB.Models.Factions.FactionModels.Audit;

namespace ExpressedRealms.DB.Models.Factions.FactionModels;

public class Faction : ISoftDelete
{
    public int Id { get; set; }
    public int ExpressionId { get; set; }
    public string Name { get; set; } = null!;
    public string Background { get; set; } = null!;

    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public virtual Expression Expression { get; set; } = null!;
    public virtual ICollection<FactionAuditTrail> FactionAuditTrails { get; set; } =
        new HashSet<FactionAuditTrail>();
    public virtual ICollection<Contact> Contacts { get; set; } = new HashSet<Contact>();
}
