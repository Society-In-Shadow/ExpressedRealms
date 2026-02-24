using Audit.EntityFramework.ConfigurationApi;
using ExpressedRealms.DB.Exceptions;
using ExpressedRealms.DB.Interceptors;

namespace ExpressedRealms.DB.UserProfile.PlayerDBModels.PlayerSetup;

internal static class PlayerAuditConfiguration
{
    public static List<ChangedRecord> ProcessChangedRecords(List<ChangedRecord> changedRecords)
    {
        List<ChangedRecord> changedRecordsToReturn = new();
        foreach (var changedRecord in changedRecords)
        {
            switch (changedRecord.ColumnName)
            {
                case "name":
                    changedRecord.FriendlyName = "Player Name";
                    break;

                case "player_number":
                    changedRecord.FriendlyName = "Player Number";
                    break;

                case "user_id":
                case "lookup_id":
                    continue;

                default:
                    throw new MissingAuditColumnException(changedRecord.ColumnName);
            }

            changedRecordsToReturn.Add(changedRecord);
        }

        return changedRecordsToReturn;
    }

    public static IAuditEntityMapping AddPlayerAuditTrailMapping(this IAuditEntityMapping mapping)
    {
        return mapping.Map<Player, PlayerAuditTrail>(
            (player, audit) =>
            {
                audit.PlayerId = player.Id;
                return true;
            }
        );
    }
}
