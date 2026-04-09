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
                case "is_archetype_account":
                    continue;

                case "send_pickup_crb_email":
                    changedRecord.FriendlyName = "Send Pickup CRB Email";
                    break;

                case "has_signed_consent_form":
                    changedRecord.FriendlyName = "Consent Form Signed";
                    break;

                case "last_age_group_check":
                    changedRecord.FriendlyName = "Age Group Checked";
                    break;

                case "age_group_id":
                    changedRecord.FriendlyName = "Age Group";
                    break;

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
