using Audit.EntityFramework.ConfigurationApi;
using ExpressedRealms.DB.Exceptions;
using ExpressedRealms.DB.Interceptors;

namespace ExpressedRealms.DB.Models.Expressions.ExpressionSetup;

internal static class ExpressionAuditConfiguration
{
    public static List<ChangedRecord> ProcessChangedRecords(List<ChangedRecord> changedRecords)
    {
        List<ChangedRecord> changedRecordsToReturn = new();
        foreach (var changedRecord in changedRecords)
        {
            switch (changedRecord.ColumnName)
            {
                case "expression_type_id":
                    continue;

                case "name":
                    break;

                case "short_description":
                    changedRecord.FriendlyName = "Short Description";
                    break;

                case "nav_menu_item":
                    changedRecord.FriendlyName = "Navigation Menu Image";
                    break;

                case "publish_status_id":
                    changedRecord.FriendlyName = "Publish Status";
                    break;

                case "order_index":
                    changedRecord.FriendlyName = "Sort Order";
                    break;

                default:
                    throw new MissingAuditColumnException(changedRecord.ColumnName);
            }
        }

        return changedRecordsToReturn;
    }

    public static IAuditEntityMapping AddExpressionAuditTrailMapping(
        this IAuditEntityMapping mapping
    )
    {
        return mapping.Map<Expression, ExpressionAuditTrail>(
            (section, audit) =>
            {
                audit.ExpressionId = section.Id;
                return true;
            }
        );
    }
}
