using Audit.EntityFramework.ConfigurationApi;
using ExpressedRealms.DB.Exceptions;
using ExpressedRealms.DB.Interceptors;

namespace ExpressedRealms.DB.Models.Knowledges.KnowledgeModels.Audit;

internal static class KnowledgesAuditTrailExtensions
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

                case "knowledge_type_id":
                    changedRecord.FriendlyName = "Knowledge Type";
                    break;

                default:
                    throw new MissingAuditColumnException(changedRecord.ColumnName);
            }

            changedRecordsToReturn.Add(changedRecord);
        }

        return changedRecordsToReturn;
    }

    public static IAuditEntityMapping AddKnowledgeAuditTrailMapping(
        this IAuditEntityMapping mapping
    )
    {
        return mapping.Map<Knowledge, KnowledgeAuditTrail>(
            (powerPath, audit) =>
            {
                audit.KnowledgeId = powerPath.Id;
                return true;
            }
        );
    }
}
