using Audit.EntityFramework.ConfigurationApi;
using ExpressedRealms.DB.Exceptions;
using ExpressedRealms.DB.Interceptors;

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
                case "user_id":
                    continue;
                case "role_id":
                    changedRecord.ColumnName = "Role";
                    changedRecord.Message = "Role was updated";
                    changedRecord.NewValue = null;
                    changedRecord.OriginalValue = null;
                    break;
                default:
                    throw new MissingAuditColumnException(changedRecord.ColumnName);
            }
            changedRecordsToReturn.Add(changedRecord);
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
