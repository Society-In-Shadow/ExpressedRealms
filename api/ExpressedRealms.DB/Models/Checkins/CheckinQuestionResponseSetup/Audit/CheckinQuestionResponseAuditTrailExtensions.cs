using Audit.EntityFramework.ConfigurationApi;
using ExpressedRealms.DB.Exceptions;
using ExpressedRealms.DB.Interceptors;

namespace ExpressedRealms.DB.Models.Checkins.CheckinQuestionResponseSetup.Audit;

internal static class CheckinQuestionResponseAuditTrailExtensions
{
    public static List<ChangedRecord> ProcessChangedRecords(List<ChangedRecord> changedRecords)
    {
        List<ChangedRecord> changedRecordsToReturn = new();
        foreach (var changedRecord in changedRecords)
        {
            switch (changedRecord.ColumnName)
            {
                case "event_question_id":
                case "checkin_id":
                    continue;

                case "response":
                    changedRecord.FriendlyName = "Response";
                    break;

                default:
                    throw new MissingAuditColumnException(changedRecord.ColumnName);
            }

            changedRecordsToReturn.Add(changedRecord);
        }

        return changedRecordsToReturn;
    }

    public static IAuditEntityMapping AddCheckinQuestionResponseAuditTrailMapping(
        this IAuditEntityMapping mapping
    )
    {
        return mapping.Map<CheckinQuestionResponse, CheckinQuestionResponseAuditTrail>(
            (model, audit) =>
            {
                audit.EventQuestionId = model.EventQuestionId;
                audit.CheckinId = model.CheckinId;
                return true;
            }
        );
    }
}
