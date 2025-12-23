using Audit.EntityFramework.ConfigurationApi;
using ExpressedRealms.DB.Exceptions;
using ExpressedRealms.DB.Interceptors;

namespace ExpressedRealms.DB.Models.Authorization.UserRoleMappingSetup.Audit;

internal static class UserRoleMappingAuditTrailExtensions
{
    public static List<ChangedRecord> ProcessChangedRecords(List<ChangedRecord> changedRecords)
    {
        List<ChangedRecord> changedRecordsToReturn = new();
        foreach (var changedRecord in changedRecords)
        {
            switch (changedRecord.ColumnName)
            {
                case "role_id":
                case "user_id":
                    break;

                case "expire_date":
                    changedRecord.FriendlyName = "Expire Date";
                    break;

                default:
                    throw new MissingAuditColumnException(changedRecord.ColumnName);
            }

            changedRecordsToReturn.Add(changedRecord);
        }

        return changedRecordsToReturn;
    }

    public static IAuditEntityMapping AddUserRoleMappingAuditTrailMapping(
        this IAuditEntityMapping mapping
    )
    {
        return mapping.Map<UserRoleMapping, UserRoleMappingAuditTrail>(
            (dbModel, audit) =>
            {
                audit.UserRoleMappingId = dbModel.Id;
                audit.RoleId = dbModel.RoleId;
                audit.UserId = dbModel.UserId;
                return true;
            }
        );
    }
}
