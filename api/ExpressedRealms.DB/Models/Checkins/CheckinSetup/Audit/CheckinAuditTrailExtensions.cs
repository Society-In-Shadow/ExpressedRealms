using Audit.EntityFramework.ConfigurationApi;
using ExpressedRealms.DB.Exceptions;
using ExpressedRealms.DB.Interceptors;

namespace ExpressedRealms.DB.Models.Checkins.CheckinSetup.Audit;

internal static class CheckinAuditTrailExtensions
{
    public static List<ChangedRecord> ProcessChangedRecords(List<ChangedRecord> changedRecords)
    {
        List<ChangedRecord> changedRecordsToReturn = new();
        foreach (var changedRecord in changedRecords)
        {
            switch (changedRecord.ColumnName)
            {
                case "event_id":
                case "player_id":
                    changedRecord.Message = "Verified User Details";
                    break;

                default:
                    throw new MissingAuditColumnException(changedRecord.ColumnName);
            }

            changedRecordsToReturn.Add(changedRecord);
        }

        return changedRecordsToReturn;
    }

    public static IAuditEntityMapping AddCheckinAuditTrailMapping(this IAuditEntityMapping mapping)
    {
        return mapping.Map<Checkin, CheckinAuditTrail>(
            (model, audit) =>
            {
                audit.CheckinId = model.Id;
                return true;
            }
        );
    }
}
