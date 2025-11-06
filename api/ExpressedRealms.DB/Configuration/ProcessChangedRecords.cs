using ExpressedRealms.DB.Characters.AssignedXp.AssignedXpMappingModels;
using ExpressedRealms.DB.Characters.AssignedXp.AssignedXpMappingModels.Audit;
using ExpressedRealms.DB.Characters.AssignedXp.AssignedXpTypeModels;
using ExpressedRealms.DB.Characters.AssignedXP.AssignedXpTypeModels.Audit;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Blessings.BlessingLevelSetup;
using ExpressedRealms.DB.Models.Blessings.BlessingLevelSetup.Audit;
using ExpressedRealms.DB.Models.Blessings.BlessingSetup;
using ExpressedRealms.DB.Models.Blessings.BlessingSetup.Audit;
using ExpressedRealms.DB.Models.Events.EventScheduleItemsSetup;
using ExpressedRealms.DB.Models.Events.EventScheduleItemsSetup.Audit;
using ExpressedRealms.DB.Models.Events.EventSetup;
using ExpressedRealms.DB.Models.Events.EventSetup.Audit;
using ExpressedRealms.DB.Models.Expressions.ExpressionSectionSetup;
using ExpressedRealms.DB.Models.Expressions.ExpressionSetup;
using ExpressedRealms.DB.Models.Expressions.ProgressionPathSetup.ProgressionLevels;
using ExpressedRealms.DB.Models.Expressions.ProgressionPathSetup.ProgressionLevels.Audit;
using ExpressedRealms.DB.Models.Expressions.ProgressionPathSetup.ProgressionPaths;
using ExpressedRealms.DB.Models.Expressions.ProgressionPathSetup.ProgressionPaths.Audit;
using ExpressedRealms.DB.Models.Knowledges.KnowledgeModels;
using ExpressedRealms.DB.Models.Knowledges.KnowledgeModels.Audit;
using ExpressedRealms.DB.Models.Powers;
using ExpressedRealms.DB.Models.Powers.PowerPathSetup;
using ExpressedRealms.DB.Models.Powers.PowerSetup.Audit;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.PlayerSetup;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserRoles;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;

namespace ExpressedRealms.DB.Configuration;

public static class ProcessChangedRecords
{
    public static List<ChangedRecord> ProcessRecords(
        string tableName,
        List<ChangedRecord> changedRecords
    )
    {
        return tableName switch
        {
            nameof(User) => UserAuditConfiguration.ProcessChangedRecords(changedRecords),
            nameof(ExpressionSection) => ExpressionSectionAuditConfiguration.ProcessChangedRecords(
                changedRecords
            ),
            nameof(Expression) => ExpressionAuditConfiguration.ProcessChangedRecords(
                changedRecords
            ),
            nameof(Player) => PlayerAuditConfiguration.ProcessChangedRecords(changedRecords),
            nameof(UserRole) => UserRoleAuditConfiguration.ProcessChangedRecords(changedRecords),
            nameof(PowerPath) => PowerPathAuditTrailExtensions.ProcessChangedRecords(
                changedRecords
            ),
            nameof(Power) => PowerAuditTrailExtensions.ProcessChangedRecords(changedRecords),
            nameof(Knowledge) => KnowledgesAuditTrailExtensions.ProcessChangedRecords(
                changedRecords
            ),
            nameof(Blessing) => BlessingAuditTrailExtensions.ProcessChangedRecords(changedRecords),
            nameof(BlessingLevel) => BlessingLevelAuditTrailExtensions.ProcessChangedRecords(
                changedRecords
            ),
            nameof(ProgressionPath) => ProgressionPathAuditTrailExtensions.ProcessChangedRecords(
                changedRecords
            ),
            nameof(ProgressionLevel) => ProgressionLevelAuditTrailExtensions.ProcessChangedRecords(
                changedRecords
            ),
            nameof(Event) => EventAuditTrailExtensions.ProcessChangedRecords(changedRecords),
            nameof(EventScheduleItem) =>
                EventScheduleItemAuditTrailExtensions.ProcessChangedRecords(changedRecords),
            nameof(AssignedXpMapping) => AssignedXpMappingsAuditTrailExtensions.ProcessChangedRecords(changedRecords),
            nameof(AssignedXpType) => AssignedXpTypesAuditTrailExtensions.ProcessChangedRecords(changedRecords),
            _ => throw new ArgumentException(
                $"Table not setup in the ProcessChangedRecords class: {tableName}"
            ),
        };
    }
}
