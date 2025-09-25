using Audit.EntityFramework.ConfigurationApi;
using ExpressedRealms.DB.Exceptions;
using ExpressedRealms.DB.Interceptors;

namespace ExpressedRealms.DB.Models.Expressions.ProgressionPathSetup.ProgressionLevels.Audit;

internal static class ProgressionLevelAuditTrailExtensions
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

    public static IAuditEntityMapping AddProgressionLevelAuditTrailMapping(
        this IAuditEntityMapping mapping
    )
    {
        return mapping.Map<ProgressionLevel, ProgressionLevelAuditTrail>(
            (model, audit) =>
            {
                audit.ProgressionPathId = model.ProgressionPathId;
                audit.ProgressionLevelId = model.Id;
                return true;
            }
        );
    }
}
