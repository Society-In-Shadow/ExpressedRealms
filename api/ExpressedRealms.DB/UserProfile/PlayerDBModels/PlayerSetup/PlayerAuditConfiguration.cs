using Audit.EntityFramework.ConfigurationApi;
using ExpressedRealms.DB.Interceptors;

namespace ExpressedRealms.DB.UserProfile.PlayerDBModels.PlayerSetup;

internal static class PlayerAuditConfiguration
{
    public static List<ChangedRecord> ProcessChangedRecords(List<ChangedRecord> changedRecords)
    {
        List<ChangedRecord> changedRecordsToReturn = new();
        foreach (var changedRecord in changedRecords)
        {
            var skipRecord = false;
            switch (changedRecord.ColumnName)
            {
                case nameof(Player.Name):
                    changedRecord.FriendlyName = "Player Name";
                    break;

                case nameof(Player.UserId):
                    skipRecord = true;
                    break;

                default:
                    throw new Exception($"Unknown column name {changedRecord.ColumnName}");
            }

            if (skipRecord)
                continue;

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
