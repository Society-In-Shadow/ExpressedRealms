using Audit.EntityFramework.ConfigurationApi;
using ExpressedRealms.DB.Exceptions;
using ExpressedRealms.DB.Interceptors;

namespace ExpressedRealms.DB.Models.Powers.PowerSetup.Audit;

internal static class PowerAuditTrailExtensions
{
    public static List<ChangedRecord> ProcessChangedRecords(List<ChangedRecord> changedRecords)
    {
        List<ChangedRecord> changedRecordsToReturn = new();
        foreach (var changedRecord in changedRecords)
        {
            switch (changedRecord.ColumnName)
            {
                case "id":
                case "power_path_id":
                    continue;

                case "name":
                    changedRecord.FriendlyName = "Name";
                    break;

                case "description":
                    changedRecord.FriendlyName = "Description";
                    break;

                case "level_id":
                    changedRecord.FriendlyName = "Power Level";
                    break;

                case "area_of_effect_type_id":
                    changedRecord.FriendlyName = "Area of Effect";
                    break;

                case "activation_timing_type_id":
                    changedRecord.FriendlyName = "Activation Timing";
                    break;

                case "duration_id":
                    changedRecord.FriendlyName = "Duration";
                    break;

                case "is_power_use":
                    changedRecord.FriendlyName = "Is Power Use";
                    break;

                case "game_mechanic_effect":
                    changedRecord.FriendlyName = "Game Mechanic Effect";
                    break;

                case "limitation":
                    changedRecord.FriendlyName = "Limitation";
                    break;

                case "other_fields":
                    changedRecord.FriendlyName = "Other";
                    break;

                case "cost":
                    changedRecord.FriendlyName = "Cost";
                    break;

                case "order_index":
                    changedRecord.FriendlyName = "Sort Order";
                    break;

                case "stat_modifier_group":
                    changedRecord.Message = "Added a stat modifier group";
                    break;

                default:
                    throw new MissingAuditColumnException(changedRecord.ColumnName);
            }

            changedRecordsToReturn.Add(changedRecord);
        }

        return changedRecordsToReturn;
    }

    public static IAuditEntityMapping AddPowerAuditTrailMapping(this IAuditEntityMapping mapping)
    {
        return mapping.Map<Power, PowerAuditTrail>(
            (table, audit) =>
            {
                audit.PowerId = table.Id;
                audit.PowerPathId = table.PowerPathId;
                return true;
            }
        );
    }
}
