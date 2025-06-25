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
            var skipRecord = false;
            switch (changedRecord.ColumnName)
            {
                case nameof(Power.Id):
                case "power_path_id":
                    skipRecord = true;
                    break;

                case nameof(Power.Name):
                    changedRecord.FriendlyName = "Name";
                    break;

                case nameof(Power.Description):
                    changedRecord.FriendlyName = "Description";
                    break;

                case nameof(Power.LevelId):
                    changedRecord.FriendlyName = "Power Level";
                    break;

                case nameof(Power.AreaOfEffectTypeId):
                    changedRecord.FriendlyName = "Area of Effect";
                    break;

                case nameof(Power.ActivationTimingTypeId):
                    changedRecord.FriendlyName = "Activation Timing";
                    break;

                case nameof(Power.DurationId):
                    changedRecord.FriendlyName = "Duration";
                    break;

                case nameof(Power.IsPowerUse):
                    changedRecord.FriendlyName = "Is Power Use";
                    break;

                case nameof(Power.GameMechanicEffect):
                    changedRecord.FriendlyName = "Game Mechanic Effect";
                    break;

                case nameof(Power.Limitation):
                    changedRecord.FriendlyName = "Limitation";
                    break;

                case nameof(Power.OtherFields):
                    changedRecord.FriendlyName = "Other";
                    break;

                case "cost":
                    changedRecord.FriendlyName = "Cost";
                    break;

                case "order_index":
                    changedRecord.FriendlyName = "Sort Order";
                    break;

                default:
                    throw new MissingAuditColumnException(changedRecord.ColumnName);
            }

            if (skipRecord)
                continue;

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
