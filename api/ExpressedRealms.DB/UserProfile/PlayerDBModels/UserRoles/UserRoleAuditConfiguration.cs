using Audit.EntityFramework.ConfigurationApi;
using ExpressedRealms.DB.Interceptors;
using Microsoft.AspNetCore.Identity;

namespace ExpressedRealms.DB.UserProfile.PlayerDBModels.UserRoles;

internal static class UserRoleAuditConfiguration
{
    public static List<ChangedRecord> ProcessChangedRecords(List<ChangedRecord> changedRecords)
    {
        List<ChangedRecord> changedRecordsToReturn = new();
        foreach (var changedRecord in changedRecords)
        {
            switch (changedRecord.ColumnName)
            {
                case nameof(IdentityUserRole<string>.UserId):
                    break;
                case nameof(IdentityUserRole<string>.RoleId):
                    changedRecord.ColumnName = "Role";
                    changedRecord.Message = "Role was updated";
                    changedRecord.NewValue = null;
                    changedRecord.OriginalValue = null;
                    changedRecordsToReturn.Add(changedRecord);
                    break;
                default:
                    throw new Exception($"Unknown column name {changedRecord.ColumnName}");
            }
        }

        return changedRecordsToReturn;
    }

    public static IAuditEntityMapping AddUserRoleAuditTrailMapping(this IAuditEntityMapping mapping)
    {
        return mapping.Map<UserRole, UserRoleAuditTrail>(
            (role, audit) =>
            {
                audit.RoleId = role.RoleId;
                audit.MappingUserId = role.UserId;
                return true;
            }
        );
    }
}
