using Audit.EntityFramework.ConfigurationApi;
using ExpressedRealms.DB.Exceptions;
using ExpressedRealms.DB.Interceptors;

namespace ExpressedRealms.DB.Models.Events.EventSetup.Audit;

internal static class EventAuditTrailExtensions
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

                case "start_date":
                    changedRecord.FriendlyName = "Start Date";
                    break;

                case "end_date":
                    changedRecord.FriendlyName = "End Date";
                    break;

                case "location":
                    changedRecord.FriendlyName = "Location";
                    break;

                case "website_name":
                    changedRecord.FriendlyName = "Website Name";
                    break;
                
                case "website_url":
                    changedRecord.FriendlyName = "Website URL";
                    break;
                
                case "additional_notes":
                    changedRecord.FriendlyName = "Website Name";
                    break;
                
                case "con_experience":
                    changedRecord.FriendlyName = "Con Experience";
                    break;
                
                default:
                    throw new MissingAuditColumnException(changedRecord.ColumnName);
            }

            changedRecordsToReturn.Add(changedRecord);
        }

        return changedRecordsToReturn;
    }

    public static IAuditEntityMapping AddEventAuditTrailMapping(this IAuditEntityMapping mapping)
    {
        return mapping.Map<Event, EventAuditTrail>(
            (Event, audit) =>
            {
                audit.EventId = Event.Id;
                return true;
            }
        );
    }
}
