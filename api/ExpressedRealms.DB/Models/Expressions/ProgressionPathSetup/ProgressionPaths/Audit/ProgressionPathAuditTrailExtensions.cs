using Audit.EntityFramework.ConfigurationApi;
using ExpressedRealms.DB.Exceptions;
using ExpressedRealms.DB.Interceptors;

namespace ExpressedRealms.DB.Models.Expressions.ProgressionPathSetup.ProgressionPaths.Audit;

internal static class ProgressionPathAuditTrailExtensions
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

                case "expression_id":
                case "progression_Id":
                    break;

                default:
                    throw new MissingAuditColumnException(changedRecord.ColumnName);
            }

            changedRecordsToReturn.Add(changedRecord);
        }

        return changedRecordsToReturn;
    }

    public static IAuditEntityMapping AddProgressionPathAuditTrailMapping(
        this IAuditEntityMapping mapping
    )
    {
        return mapping.Map<ProgressionPath, ProgressionPathAuditTrail>(
            (model, audit) =>
            {
                audit.ProgressionPathId = model.Id;
                audit.ExpressionId = model.ExpressionId;
                return true;
            }
        );
    }
}
