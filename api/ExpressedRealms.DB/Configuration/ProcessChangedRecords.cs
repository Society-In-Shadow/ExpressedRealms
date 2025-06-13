using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Expressions.ExpressionSectionSetup;
using ExpressedRealms.DB.Models.Expressions.ExpressionSetup;
using ExpressedRealms.DB.Models.Powers.PowerPathSetup;
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
            nameof(PowerPath) => PowerPathAuditConfiguration.ProcessChangedRecords(changedRecords),
            _ => throw new ArgumentException(
                $"Table not setup in the ProcessChangedRecords class: {tableName}"
            ),
        };
    }
}
