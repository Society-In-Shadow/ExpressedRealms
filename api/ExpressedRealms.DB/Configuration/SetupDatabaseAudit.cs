using System.Text.Json;
using Audit.Core;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Expressions.ExpressionSectionSetup;
using ExpressedRealms.DB.Models.Expressions.ExpressionSetup;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.PlayerSetup;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;

namespace ExpressedRealms.DB.Configuration;

public static class SetupDatabaseAudit
{
    public static void SetupAudit()
    {
        var globallyExcludedColumns = new List<string>() { "Id", "DeletedAt", "IsDeleted" };
        Audit
            .Core.Configuration.Setup()
            .UseEntityFramework(x =>
                x.AuditTypeExplicitMapper(m =>
                        m.AddExpressionSectionAuditTrailMapping()
                         .AddExpressionAuditTrailMapping()
                         .AddUserAuditTrailMapping()
                         .AddPlayerAuditTrailMapping()
                         .AuditEntityAction<IAuditTable>(
                            (evt, entry, audit) =>
                            {
                                audit.Action = entry.Action;
                                audit.Timestamp = DateTime.UtcNow;
                                
                                // Need to handle edge case of a user being created
                                if (!evt.CustomFields.ContainsKey("UserId"))
                                {
                                    audit.UserId = ExtractUserId(entry.EntityType.Name, entry.ColumnValues);
                                }
                                else
                                {
                                    audit.UserId = evt.CustomFields["UserId"]?.ToString();
                                }
                                
                                // TODO: Need a delete clause in here, if soft delete is enabled, say delete as an action

                                var changes = new List<ChangedRecord>();
                                if (
                                    string.Compare(
                                        audit.Action,
                                        "insert",
                                        StringComparison.InvariantCultureIgnoreCase
                                    ) == 0
                                )
                                {
                                    changes = entry.ColumnValues
                                        .Where(x =>
                                            !globallyExcludedColumns.Contains(x.Key)
                                        )
                                        .Select(x => new ChangedRecord()
                                        {
                                            ColumnName = x.Key,
                                            OriginalValue = null,
                                            NewValue = x.Value?.ToString()
                                        }).ToList();
                                }
                                else
                                {
                                    changes = entry
                                        .Changes.Where(x =>
                                            x.NewValue == null
                                                ? x.OriginalValue != null
                                                : !x.NewValue.Equals(x.OriginalValue)
                                        )
                                        .Select(change => new ChangedRecord()
                                        {
                                            ColumnName = change.ColumnName,
                                            OriginalValue = change.OriginalValue?.ToString(),
                                            NewValue = change.NewValue?.ToString()
                                        }).ToList();
                                }

                                var processedRecords = 
                                    ProcessChangedRecords.ProcessRecords(entry.EntityType.Name, changes);

                                if (!processedRecords.Any())
                                    return false;
                                
                                audit.ChangedProperties = JsonSerializer.Serialize(processedRecords);

                                return true;
                            }
                        )
                    )
                    .IgnoreMatchedProperties(true)
            );
    }
    
    private static string ExtractUserId(string entityTypeName, IDictionary<string, object> columnValues)
    {
        return entityTypeName switch
        {
            nameof(User) => columnValues.First(x => x.Key == "Id").Value.ToString(),
            nameof(Player) => columnValues.First(x => x.Key == "UserId").Value.ToString(),
            _ => throw new InvalidOperationException($"Unsupported entity type: {entityTypeName}")
        };
    }

}
