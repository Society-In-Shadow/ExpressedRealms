using Audit.EntityFramework.ConfigurationApi;
using ExpressedRealms.DB.Interceptors;

namespace ExpressedRealms.DB.Models.Powers.PowerPathSetup;

internal static class PowerPathAuditConfiguration
{
    public static List<ChangedRecord> ProcessChangedRecords(List<ChangedRecord> changedRecords)
    {
        List<ChangedRecord> changedRecordsToReturn = new();
        foreach (var changedRecord in changedRecords)
        {
            var skipRecord = false;
            switch (changedRecord.ColumnName)
            {
                case "expression_id":
                    // You cannot change the Expression Id after creation
                    skipRecord = true;
                    break;

                case "name":
                    changedRecord.FriendlyName = "Name";
                    break;

                case "description":
                    changedRecord.FriendlyName = "Description";
                    break;

                default:
                    throw new Exception($"Unknown column name {changedRecord.ColumnName}");
            }

            if (skipRecord)
                continue;

            changedRecordsToReturn.Add(changedRecord);
        }

        return changedRecordsToReturn;
    }

    public static IAuditEntityMapping AddPowerPathAuditTrailMapping(
        this IAuditEntityMapping mapping
    )
    {
        return mapping.Map<PowerPath, PowerPathAuditTrail>(
            (powerPath, audit) =>
            {
                audit.ExpressionId = powerPath.ExpressionId;
                ;
                audit.PowerPathId = powerPath.Id;
                ;
                return true;
            }
        );
    }
}
