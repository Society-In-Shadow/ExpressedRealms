using Audit.EntityFramework.ConfigurationApi;
using ExpressedRealms.DB.Exceptions;
using ExpressedRealms.DB.Interceptors;

namespace ExpressedRealms.DB.Models.ModifierSystem.StatGroupMappings.Audit;

internal static class StatGroupMappingAuditTrailExtensions
{
    public static List<ChangedRecord> ProcessChangedRecords(List<ChangedRecord> changedRecords)
    {
        List<ChangedRecord> changedRecordsToReturn = new();
        foreach (var changedRecord in changedRecords)
        {
            switch (changedRecord.ColumnName)
            {
                case "stat_group_id":
                    continue;

                case "stat_modifier_id":
                    changedRecord.FriendlyName = "Stat Type";
                    break;

                case "modifier":
                    changedRecord.FriendlyName = "Modifier";
                    break;

                case "scale_with_level":
                    changedRecord.FriendlyName = "Scale with Level";
                    break;

                case "creation_specific_bonus":
                    changedRecord.FriendlyName = "Creation Specific Bonus";
                    break;

                case "target_expression_id":
                    changedRecord.FriendlyName = "Target Expression";
                    break;

                case "target_progression_path_id":
                    changedRecord.FriendlyName = "Target Progression Path";
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

    public static IAuditEntityMapping AddStatGroupMappingAuditTrailMapping(
        this IAuditEntityMapping mapping
    )
    {
        return mapping.Map<StatGroupMapping, StatGroupMappingAuditTrail>(
            (model, audit) =>
            {
                audit.StatGroupMappingId = model.Id;
                return true;
            }
        );
    }
}
