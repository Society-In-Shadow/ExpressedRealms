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
