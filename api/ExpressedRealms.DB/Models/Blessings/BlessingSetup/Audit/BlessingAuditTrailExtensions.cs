using Audit.EntityFramework.ConfigurationApi;
using ExpressedRealms.DB.Exceptions;
using ExpressedRealms.DB.Interceptors;

namespace ExpressedRealms.DB.Models.Blessings.BlessingSetup.Audit;

internal static class BlessingAuditTrailExtensions
{
    public static List<ChangedRecord> ProcessChangedRecords(List<ChangedRecord> changedRecords)
    {
        List<ChangedRecord> changedRecordsToReturn = new();
        foreach (var changedRecord in changedRecords)
        {
            switch (changedRecord.ColumnName)
            {
                case "name":
                    changedRecord.FriendlyName = "Name";
                    break;

                case "description":
                    changedRecord.FriendlyName = "Description";
                    break;

                case "type":
                    changedRecord.FriendlyName = "Type";
                    break;

                case "sub_category":
                    changedRecord.FriendlyName = "Sub Category";
                    break;

                default:
                    throw new MissingAuditColumnException(changedRecord.ColumnName);
            }

            changedRecordsToReturn.Add(changedRecord);
        }

        return changedRecordsToReturn;
    }

    public static IAuditEntityMapping AddBlessingAuditTrailMapping(this IAuditEntityMapping mapping)
    {
        return mapping.Map<Blessing, BlessingAuditTrail>(
            (blessing, audit) =>
            {
                audit.BlessingId = blessing.Id;
                return true;
            }
        );
    }
}
