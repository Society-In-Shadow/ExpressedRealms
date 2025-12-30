using Audit.EntityFramework.ConfigurationApi;
using ExpressedRealms.DB.Exceptions;
using ExpressedRealms.DB.Interceptors;

namespace ExpressedRealms.DB.Models.Authorization.RoleSetup.Audit;

internal static class RoleAuditTrailExtensions
{
    public static List<ChangedRecord> ProcessChangedRecords(List<ChangedRecord> changedRecords)
    {
        List<ChangedRecord> changedRecordsToReturn = new();
        foreach (var changedRecord in changedRecords)
        {
            switch (changedRecord.ColumnName)
            {
                case "description":
                    changedRecord.FriendlyName = "Description";
                    break;

                case "name":
                    changedRecord.FriendlyName = "Name";
                    break;

                default:
                    throw new MissingAuditColumnException(changedRecord.ColumnName);
            }

            changedRecordsToReturn.Add(changedRecord);
        }

        return changedRecordsToReturn;
    }

    public static IAuditEntityMapping AddRoleAuditTrailMapping(this IAuditEntityMapping mapping)
    {
        return mapping.Map<Role, RoleAuditTrail>(
            (blessing, audit) =>
            {
                audit.RoleId = blessing.Id;
                return true;
            }
        );
    }
}
