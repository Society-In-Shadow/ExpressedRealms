using Audit.EntityFramework.ConfigurationApi;
using ExpressedRealms.DB.Exceptions;
using ExpressedRealms.DB.Interceptors;

namespace ExpressedRealms.DB.Characters.AssignedXp.AssignedXpMappingModels.Audit;

internal static class AssignedXpMappingsAuditTrailExtensions
{
    public static List<ChangedRecord> ProcessChangedRecords(List<ChangedRecord> changedRecords)
    {
        List<ChangedRecord> changedRecordsToReturn = new();
        foreach (var changedRecord in changedRecords)
        {
            switch (changedRecord.ColumnName)
            {
                case "character_id":
                    changedRecord.FriendlyName = "Character";
                    break;

                case "player_id":
                    changedRecord.FriendlyName = "Player";
                    break;

                case "event_id":
                    changedRecord.FriendlyName = "Event";
                    break;
                
                case "reason":
                    changedRecord.FriendlyName = "Reason";
                    break;

                default:
                    throw new MissingAuditColumnException(changedRecord.ColumnName);
            }

            changedRecordsToReturn.Add(changedRecord);
        }

        return changedRecordsToReturn;
    }

    public static IAuditEntityMapping AddAssignedXpMappingAuditTrailMapping(
        this IAuditEntityMapping mapping
    )
    {
        return mapping.Map<AssignedXpMapping, AssignedXpMappingAuditTrail>(
            (model, audit) =>
            {
                audit.AssignedXpMappingId = model.Id;
                return true;
            }
        );
    }
}
