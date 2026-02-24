using Audit.EntityFramework.ConfigurationApi;
using ExpressedRealms.DB.Exceptions;
using ExpressedRealms.DB.Interceptors;

namespace ExpressedRealms.DB.Models.Characters.AssignedXP.AssignedXpTypeModels.Audit;

internal static class AssignedXpTypesAuditTrailExtensions
{
    public static List<ChangedRecord> ProcessChangedRecords(List<ChangedRecord> changedRecords)
    {
        List<ChangedRecord> changedRecordsToReturn = new();
        foreach (var changedRecord in changedRecords)
        {
            switch (changedRecord.ColumnName)
            {
                case "name":
                    changedRecord.FriendlyName = "Name";
                    break;

                case "description":
                    changedRecord.FriendlyName = "Description";
                    break;

                default:
                    throw new MissingAuditColumnException(changedRecord.ColumnName);
            }

            changedRecordsToReturn.Add(changedRecord);
        }

        return changedRecordsToReturn;
    }

    public static IAuditEntityMapping AddAssignedXpTypeAuditTrailMapping(
        this IAuditEntityMapping mapping
    )
    {
        return mapping.Map<AssignedXpType, AssignedXpTypeAuditTrail>(
            (model, audit) =>
            {
                audit.AssignedXpTypeId = model.Id;
                return true;
            }
        );
    }
}
