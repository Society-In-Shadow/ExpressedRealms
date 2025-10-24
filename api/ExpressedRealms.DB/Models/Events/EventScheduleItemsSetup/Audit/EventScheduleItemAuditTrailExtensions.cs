using Audit.EntityFramework.ConfigurationApi;
using ExpressedRealms.DB.Exceptions;
using ExpressedRealms.DB.Interceptors;

namespace ExpressedRealms.DB.Models.Events.EventScheduleItemsSetup.Audit;

internal static class EventScheduleItemAuditTrailExtensions
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

                case "date":
                    changedRecord.FriendlyName = "Date";
                    break;

                case "start_time":
                    changedRecord.FriendlyName = "Start Time";
                    break;

                case "end_time":
                    changedRecord.FriendlyName = "End Time";
                    break;
                
                default:
                    throw new MissingAuditColumnException(changedRecord.ColumnName);
            }

            changedRecordsToReturn.Add(changedRecord);
        }

        return changedRecordsToReturn;
    }

    public static IAuditEntityMapping AddEventScheduleItemAuditTrailMapping(this IAuditEntityMapping mapping)
    {
        return mapping.Map<EventScheduleItem, EventScheduleItemAuditTrail>(
            (eventScheduleItem, audit) =>
            {
                audit.EventScheduleItemId = eventScheduleItem.Id;
                audit.EventId = eventScheduleItem.EventId;
                return true;
            }
        );
    }
}
