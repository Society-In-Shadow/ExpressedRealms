using Audit.EntityFramework.ConfigurationApi;
using ExpressedRealms.DB.Exceptions;
using ExpressedRealms.DB.Interceptors;

namespace ExpressedRealms.DB.Models.Factions.FactionModels.Audit;

internal static class FactionAuditTrailExtensions
{
    public static List<ChangedRecord> ProcessChangedRecords(List<ChangedRecord> changedRecords)
    {
        List<ChangedRecord> changedRecordsToReturn = new();
        foreach (var changedRecord in changedRecords)
        {
            switch (changedRecord.ColumnName)
            {
                case "expression_id":
                    continue;
                
                case "name":
                    changedRecord.FriendlyName = "Name";
                    break;

                case "background":
                    changedRecord.FriendlyName = "Background";
                    break;

                default:
                    throw new MissingAuditColumnException(changedRecord.ColumnName);
            }

            changedRecordsToReturn.Add(changedRecord);
        }

        return changedRecordsToReturn;
    }

    public static IAuditEntityMapping AddFactionAuditTrailMapping(
        this IAuditEntityMapping mapping
    )
    {
        return mapping.Map<Faction, FactionAuditTrail>(
            (powerPath, audit) =>
            {
                audit.FactionId = powerPath.Id;
                return true;
            }
        );
    }
}
