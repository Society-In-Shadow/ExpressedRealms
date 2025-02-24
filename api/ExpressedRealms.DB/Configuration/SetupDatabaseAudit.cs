using System.Text.Json;
using Audit.Core;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Expressions.ExpressionSectionSetup;
using ExpressedRealms.DB.Models.Expressions.ExpressionSetup;
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
                         .AuditEntityAction<IAuditTable>(
                            (evt, entry, audit) =>
                            {
                                audit.Action = entry.Action;
                                audit.Timestamp = DateTime.UtcNow;
                                audit.UserId = evt.Environment.UserName;
                                
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

                                if(processedRecords.Any())
                                    audit.ChangedProperties = JsonSerializer.Serialize(processedRecords);

                                return true;
                            }
                        )
                    )
                    .IgnoreMatchedProperties(true)
            );
    }
}
