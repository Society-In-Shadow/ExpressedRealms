using Audit.EntityFramework.ConfigurationApi;
using ExpressedRealms.DB.Exceptions;
using ExpressedRealms.DB.Interceptors;

namespace ExpressedRealms.DB.Models.Blessings.BlessingLevelSetup.Audit;

internal static class BlessingLevelAuditTrailExtensions
{
    public static List<ChangedRecord> ProcessChangedRecords(List<ChangedRecord> changedRecords)
    {
        List<ChangedRecord> changedRecordsToReturn = new();
        foreach (var changedRecord in changedRecords)
        {
            switch (changedRecord.ColumnName)
            {
                case "blessing_id":
                    continue;

                case "description":
                    changedRecord.FriendlyName = "Description";
                    break;

                case "level":
                    changedRecord.FriendlyName = "Level";
                    break;

                case "xp_cost":
                    changedRecord.FriendlyName = "XP Cost";
                    break;

                case "xp_gain":
                    changedRecord.FriendlyName = "XP Gain";
                    break;

                default:
                    throw new MissingAuditColumnException(changedRecord.ColumnName);
            }

            changedRecordsToReturn.Add(changedRecord);
        }

        return changedRecordsToReturn;
    }

    public static IAuditEntityMapping AddBlessingLevelAuditTrailMapping(
        this IAuditEntityMapping mapping
    )
    {
        return mapping.Map<BlessingLevel, BlessingLevelAuditTrail>(
            (blessing, audit) =>
            {
                audit.BlessingId = blessing.BlessingId;
                audit.BlessingLevelId = blessing.Id;
                return true;
            }
        );
    }
}
