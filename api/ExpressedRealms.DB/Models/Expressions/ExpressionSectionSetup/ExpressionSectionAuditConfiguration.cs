using Audit.EntityFramework.ConfigurationApi;
using ExpressedRealms.DB.Exceptions;
using ExpressedRealms.DB.Interceptors;

namespace ExpressedRealms.DB.Models.Expressions.ExpressionSectionSetup;

internal static class ExpressionSectionAuditConfiguration
{
    public static List<ChangedRecord> ProcessChangedRecords(List<ChangedRecord> changedRecords)
    {
        List<ChangedRecord> changedRecordsToReturn = new();
        foreach (var changedRecord in changedRecords)
        {
            var skipRecord = false;
            switch (changedRecord.ColumnName)
            {
                case nameof(ExpressionSection.ExpressionId):
                    // You cannot change the Expression Id after creation
                    skipRecord = true;
                    break;

                case nameof(ExpressionSection.SectionTypeId):
                    changedRecord.FriendlyName = "Section Type";
                    break;

                case nameof(ExpressionSection.ParentId):
                    changedRecord.FriendlyName = "Parent Section";
                    break;

                case nameof(ExpressionSection.OrderIndex):
                    changedRecord.FriendlyName = "Sort Order";
                    break;

                case nameof(ExpressionSection.Name):
                case nameof(ExpressionSection.Content):
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

    public static IAuditEntityMapping AddExpressionSectionAuditTrailMapping(
        this IAuditEntityMapping mapping
    )
    {
        return mapping.Map<ExpressionSection, ExpressionSectionAuditTrail>(
            (section, audit) =>
            {
                audit.SectionId = section.Id;
                audit.ExpressionId = section.ExpressionId;
                return true;
            }
        );
    }
}
