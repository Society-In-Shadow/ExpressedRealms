using Audit.EntityFramework.ConfigurationApi;
using ExpressedRealms.DB.Exceptions;
using ExpressedRealms.DB.Interceptors;

namespace ExpressedRealms.DB.Models.Factions.FactionLevelModels.Audit;

internal static class FactionLevelAuditTrailExtensions
{
    public static List<ChangedRecord> ProcessChangedRecords(List<ChangedRecord> changedRecords)
    {
        List<ChangedRecord> changedRecordsToReturn = new();
        foreach (var changedRecord in changedRecords)
        {
            switch (changedRecord.ColumnName)
            {
                case "faction_id":
                case "faction_rank_id":
                    continue;

                case "knowledge_id":
                    changedRecord.FriendlyName = "Knowledge";
                    break;

                case "knowledge_level_id":
                    changedRecord.FriendlyName = "Knowledge Level";
                    break;

                case "specialization":
                    changedRecord.FriendlyName = "Specialization";
                    break;
                
                default:
                    throw new MissingAuditColumnException(changedRecord.ColumnName);
            }

            changedRecordsToReturn.Add(changedRecord);
        }

        return changedRecordsToReturn;
    }

    public static IAuditEntityMapping AddFactionLevelAuditTrailMapping(this IAuditEntityMapping mapping)
    {
        return mapping.Map<FactionLevel, FactionLevelAuditTrail>(
            (model, audit) =>
            {
                audit.FactionLevelId = model.Id;
                return true;
            }
        );
    }
}
