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
            switch (changedRecord.ColumnName)
            {
                case "expression_id":
                    continue;

                case "section_type_id":
                    changedRecord.FriendlyName = "Section Type";
                    break;

                case "parent_id":
                    changedRecord.FriendlyName = "Parent Section";
                    break;

                case "order_index":
                    changedRecord.FriendlyName = "Sort Order";
                    break;

                case "name":
                case "content":
                    break;

                default:
                    throw new MissingAuditColumnException(changedRecord.ColumnName);
            }

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
