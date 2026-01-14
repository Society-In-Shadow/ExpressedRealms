using Audit.EntityFramework.ConfigurationApi;
using ExpressedRealms.DB.Exceptions;
using ExpressedRealms.DB.Interceptors;

namespace ExpressedRealms.DB.Models.Authorization.RolePermissionMappingSetup.Audit;

internal static class RolePermissionMappingAuditTrailExtensions
{
    public static List<ChangedRecord> ProcessChangedRecords(List<ChangedRecord> changedRecords)
    {
        List<ChangedRecord> changedRecordsToReturn = new();
        foreach (var changedRecord in changedRecords)
        {
            switch (changedRecord.ColumnName)
            {
                case "role_id":
                case "permission_id":
                    break;

                default:
                    throw new MissingAuditColumnException(changedRecord.ColumnName);
            }

            changedRecordsToReturn.Add(changedRecord);
        }

        return changedRecordsToReturn;
    }

    public static IAuditEntityMapping AddRolePermissionMappingAuditTrailMapping(
        this IAuditEntityMapping mapping
    )
    {
        return mapping.Map<RolePermissionMapping, RolePermissionMappingAuditTrail>(
            (dbModel, audit) =>
            {
                audit.RolePermissionMappingId = dbModel.Id != 0 ? dbModel.Id : null;
                audit.RoleId = dbModel.RoleId;
                audit.PermissionId = dbModel.PermissionId;
                return true;
            }
        );
    }
}
