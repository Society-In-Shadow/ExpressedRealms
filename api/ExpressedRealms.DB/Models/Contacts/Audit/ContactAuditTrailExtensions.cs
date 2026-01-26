using Audit.EntityFramework.ConfigurationApi;
using ExpressedRealms.DB.Exceptions;
using ExpressedRealms.DB.Interceptors;

namespace ExpressedRealms.DB.Models.Contacts.Audit;

internal static class ContactAuditTrailExtensions
{
    public static List<ChangedRecord> ProcessChangedRecords(List<ChangedRecord> changedRecords)
    {
        List<ChangedRecord> changedRecordsToReturn = new();
        foreach (var changedRecord in changedRecords)
        {
            switch (changedRecord.ColumnName)
            {
                case "contact_id":
                case "character_id":
                    break;

                case "name":
                    changedRecord.FriendlyName = "Name";
                    break;

                case "knowledge_level_id":
                    changedRecord.FriendlyName = "Knowledge Level";
                    break;

                case "knowledge_id":
                    changedRecord.FriendlyName = "Knowledge";
                    break;

                case "frequency":
                    changedRecord.FriendlyName = "Frequency";
                    break;

                case "spent_xp":
                    changedRecord.FriendlyName = "Spent XP";
                    break;

                case "is_approved":
                    changedRecord.FriendlyName = "Approved";
                    break;

                case "notes":
                    changedRecord.FriendlyName = "Notes";
                    break;

                default:
                    throw new MissingAuditColumnException(changedRecord.ColumnName);
            }

            changedRecordsToReturn.Add(changedRecord);
        }

        return changedRecordsToReturn;
    }

    public static IAuditEntityMapping AddContactAuditTrailMapping(this IAuditEntityMapping mapping)
    {
        return mapping.Map<Contact, ContactAuditTrail>(
            (dbModel, audit) =>
            {
                audit.ContactId = dbModel.Id != 0 ? dbModel.Id : null;
                return true;
            }
        );
    }
}
