using Audit.EntityFramework;
using ExpressedRealms.DB.Characters;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Expressions.ExpressionSectionSetup;
using ExpressedRealms.DB.Models.Expressions.ProgressionPathSetup.ProgressionPaths;
using ExpressedRealms.DB.Models.Expressions.ProgressionPathSetup.ProgressionPaths.Audit;
using ExpressedRealms.DB.Models.Powers.PowerPathSetup;

namespace ExpressedRealms.DB.Models.Expressions.ExpressionSetup;

[AuditInclude]
public class Expression : ISoftDelete
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string ShortDescription { get; set; } = null!;
    public string NavMenuImage { get; set; } = null!;
    public int PublishStatusId { get; set; }
    public int ExpressionTypeId { get; set; }
    public int OrderIndex { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public virtual ExpressionPublishStatus PublishStatus { get; set; } = null!;
    public virtual ExpressionType ExpressionType { get; set; } = null!;
    public virtual List<ExpressionSection> ExpressionSections { get; set; } = null!;
    public virtual List<Character> Characters { get; set; } = null!;
    public virtual List<ExpressionSectionAuditTrail> SectionAudits { get; set; } = null!;
    public virtual List<ExpressionAuditTrail> ExpressionAudits { get; set; } = null!;
    public virtual List<PowerPath> PowerPaths { get; set; } = null!;
    public virtual List<PowerPathAuditTrail> PowerPathAudits { get; set; } = null!;
    public virtual List<ProgressionPathAuditTrail> ProgressionPathAudits { get; set; } = null!;
    public virtual List<ProgressionPath> ProgressionPaths { get; set; } = null!;
}
