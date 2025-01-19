using Audit.EntityFramework;
using ExpressedRealms.DB.Characters;
using ExpressedRealms.DB.Interceptors;

namespace ExpressedRealms.DB.Models.Expressions;

[AuditInclude]
public class Expression : ISoftDelete
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string ShortDescription { get; set; } = null!;
    public string NavMenuImage { get; set; } = null!;
    public int PublishStatusId { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public virtual ExpressionPublishStatus PublishStatus { get; set; } = null!;
    public virtual List<ExpressionSection> ExpressionSections { get; set; } = null!;
    public virtual List<Character> Characters { get; set; } = null!;
    public virtual List<ExpressionSectionAuditTrail> SectionAudits { get; set; } = null!;
    public virtual List<ExpressionAuditTrail> ExpressionAudits { get; set; } = null!;
}
