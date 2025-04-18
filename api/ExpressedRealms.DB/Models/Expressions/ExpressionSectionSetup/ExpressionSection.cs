using Audit.EntityFramework;
using ExpressedRealms.DB.Characters;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Expressions.ExpressionSetup;

namespace ExpressedRealms.DB.Models.Expressions.ExpressionSectionSetup;

[AuditInclude]
public class ExpressionSection : ISoftDelete
{
    public int Id { get; set; }
    public int ExpressionId { get; set; }
    public int SectionTypeId { get; set; }
    public int? ParentId { get; set; }
    public int OrderIndex { get; set; }
    public string Name { get; set; } = null!;
    public string Content { get; set; } = null!;
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public virtual Expression Expression { get; set; } = null!;
    public virtual ExpressionSection? Parent { get; set; }
    public virtual ExpressionSectionType SectionType { get; set; } = null;
    public virtual List<ExpressionSection>? Children { get; set; }
    public virtual List<Character> CharactersList { get; set; }
    public virtual List<ExpressionSectionAuditTrail> SectionAudits { get; set; } = null!;
}
