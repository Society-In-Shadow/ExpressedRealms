using Audit.EntityFramework;
using ExpressedRealms.DB.Models.Expressions.ExpressionSectionSetup;
using ExpressedRealms.DB.Models.Expressions.ExpressionSetup;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.PlayerSetup;
using Microsoft.AspNetCore.Identity;

namespace ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;

[AuditInclude]
public class User : IdentityUser
{
    public virtual Player? Player { get; set; }
    public virtual List<ExpressionSectionAuditTrail> ExpressionSectionAuditTrails { get; set; } =
        new();
    public virtual List<ExpressionAuditTrail> ExpressionAuditTrails { get; set; } = new();
    public virtual List<UserAuditTrail> UserAuditTrails { get; set; } = new();
    public virtual List<PlayerAuditTrail> PlayerAuditTrails { get; set; } = new();
}
