using Audit.EntityFramework;
using ExpressedRealms.DB.Models.Blessings.BlessingLevelSetup.Audit;
using ExpressedRealms.DB.Models.Blessings.BlessingSetup.Audit;
using ExpressedRealms.DB.Models.Events.EventScheduleItemsSetup.Audit;
using ExpressedRealms.DB.Models.Events.EventSetup.Audit;
using ExpressedRealms.DB.Models.Expressions.ExpressionSectionSetup;
using ExpressedRealms.DB.Models.Expressions.ExpressionSetup;
using ExpressedRealms.DB.Models.Expressions.ProgressionPathSetup.ProgressionLevels.Audit;
using ExpressedRealms.DB.Models.Expressions.ProgressionPathSetup.ProgressionPaths.Audit;
using ExpressedRealms.DB.Models.Knowledges.KnowledgeModels.Audit;
using ExpressedRealms.DB.Models.Powers.PowerPathSetup;
using ExpressedRealms.DB.Models.Powers.PowerSetup.Audit;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.PlayerSetup;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserRoles;
using Microsoft.AspNetCore.Identity;

namespace ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;

[AuditInclude]
public class User : IdentityUser
{
    public virtual Player? Player { get; set; }
    public virtual List<ExpressionSectionAuditTrail> ExpressionSectionAuditTrails { get; set; } =
        new();
    public virtual List<ExpressionAuditTrail> ExpressionAuditTrails { get; set; } = new();
    public virtual List<UserAuditTrail> UserActorAuditTrails { get; set; } = new();
    public virtual List<UserAuditTrail> UserAuditTrails { get; set; } = new();
    public virtual List<PlayerAuditTrail> PlayerAuditTrails { get; set; } = new();
    public virtual List<UserRoleAuditTrail> UserRoleAuditTrails { get; set; } = new();
    public virtual List<UserRoleAuditTrail> MappedUserRoleAuditTrails { get; set; } = new();
    public virtual List<PowerPathAuditTrail> PowerPathAudits { get; set; } = new();
    public virtual List<PowerAuditTrail> PowerAuditTrails { get; set; } = new();
    public virtual List<KnowledgeAuditTrail> KnowledgeAuditTrails { get; set; } = new();
    public virtual List<BlessingAuditTrail> BlessingAuditTrails { get; set; } = new();
    public virtual List<BlessingLevelAuditTrail> BlessingLevelAuditTrails { get; set; } = new();
    public virtual List<ProgressionPathAuditTrail> ProgressionPathAuditTrails { get; set; } = new();
    public virtual List<EventAuditTrail> EventAuditTrails { get; set; } = new();
    public virtual List<EventScheduleItemAuditTrail> EventScheduleItemAuditTrails { get; set; } =
        new();
    public virtual List<ProgressionLevelAuditTrail> ProgressionLevelAuditTrails { get; set; } =
        new();
}
