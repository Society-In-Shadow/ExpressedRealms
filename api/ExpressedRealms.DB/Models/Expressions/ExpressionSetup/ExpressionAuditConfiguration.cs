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
                case nameof(Expression.Name):
                    break;

                case nameof(Expression.ShortDescription):
                    changedRecord.FriendlyName = "Short Description";
                    break;

                case nameof(Expression.NavMenuImage):
                    changedRecord.FriendlyName = "Navigation Menu Image";
                    break;

                case nameof(Expression.PublishStatusId):
                    changedRecord.FriendlyName = "Publish Status";
                    break;

                default:
                    throw new MissingAuditColumnException(changedRecord.ColumnName);
            }

            changedRecordsToReturn.Add(changedRecord);
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
