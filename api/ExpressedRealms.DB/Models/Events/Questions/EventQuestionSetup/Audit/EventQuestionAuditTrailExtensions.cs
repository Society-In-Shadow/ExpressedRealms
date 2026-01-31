using Audit.EntityFramework.ConfigurationApi;
using ExpressedRealms.DB.Exceptions;
using ExpressedRealms.DB.Interceptors;

namespace ExpressedRealms.DB.Models.Events.Questions.EventQuestionSetup.Audit;

internal static class EventQuestionAuditTrailExtensions
{
    public static List<ChangedRecord> ProcessChangedRecords(List<ChangedRecord> changedRecords)
    {
        List<ChangedRecord> changedRecordsToReturn = new();
        foreach (var changedRecord in changedRecords)
        {
            switch (changedRecord.ColumnName)
            {
                case "event_id":
                case "question_type_id":
                    continue;

                case "question":
                    changedRecord.FriendlyName = "Question";
                    break;

                default:
                    throw new MissingAuditColumnException(changedRecord.ColumnName);
            }

            changedRecordsToReturn.Add(changedRecord);
        }

        return changedRecordsToReturn;
    }

    public static IAuditEntityMapping AddEventQuestionAuditTrailMapping(this IAuditEntityMapping mapping)
    {
        return mapping.Map<EventQuestion, EventQuestionAuditTrail>(
            (model, audit) =>
            {
                audit.EventQuestionId = model.Id;
                return true;
            }
        );
    }
}
