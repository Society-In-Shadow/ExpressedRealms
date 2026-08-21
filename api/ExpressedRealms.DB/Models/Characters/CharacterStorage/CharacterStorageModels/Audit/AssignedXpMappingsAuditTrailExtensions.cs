using Audit.EntityFramework.ConfigurationApi;
using ExpressedRealms.DB.Exceptions;
using ExpressedRealms.DB.Interceptors;

namespace ExpressedRealms.DB.Models.Characters.CharacterStorage.CharacterStorageModels.Audit;

internal static class CharacterStorageInfoAuditTrailExtensions
{
    public static List<ChangedRecord> ProcessChangedRecords(List<ChangedRecord> changedRecords)
    {
        List<ChangedRecord> changedRecordsToReturn = new();
        foreach (var changedRecord in changedRecords)
        {
            switch (changedRecord.ColumnName)
            {
                case "opted_in":
                    changedRecord.FriendlyName = "Opted In";
                    break;

                case "player_id":
                    changedRecord.FriendlyName = "Player";
                    break;

                case "event_id":
                    changedRecord.FriendlyName = "Event";
                    break;

                case "amount":
                    changedRecord.FriendlyName = "Amount";
                    break;

                case "collector_player_id":
                    changedRecord.FriendlyName = "Collector Player";
                    break;

                case "sign_off_player_id":
                    changedRecord.FriendlyName = "Sign Off Player";
                    break;

                default:
                    throw new MissingAuditColumnException(changedRecord.ColumnName);
            }

            changedRecordsToReturn.Add(changedRecord);
        }

        return changedRecordsToReturn;
    }

    public static IAuditEntityMapping AddCharacterStorageInfoAuditTrailMapping(
        this IAuditEntityMapping mapping
    )
    {
        return mapping.Map<CharacterStorageInfo, CharacterStorageInfoAuditTrail>(
            (model, audit) =>
            {
                audit.CharacterStorageInfoId = model.Id;
                return true;
            }
        );
    }
}
