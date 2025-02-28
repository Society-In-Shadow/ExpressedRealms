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
        var globallyExcludedColumns = new List<string>()
        {
            "Id",
            nameof(ISoftDelete.IsDeleted),
            nameof(ISoftDelete.DeletedAt),
        };
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
                                        audit.UserId = ExtractUserId(
                                            entry.EntityType.Name,
                                            entry.ColumnValues
                                        );
                                    }
                                    else
                                    {
                                        audit.UserId = evt.CustomFields["UserId"]?.ToString();
                                    }

                                    var changes = new List<ChangedRecord>();
                                    if (
                                        string.Compare(
                                            audit.Action,
                                            "insert",
                                            StringComparison.InvariantCultureIgnoreCase
                                        ) == 0
                                    )
                                    {
                                        changes = entry
                                            .ColumnValues.Where(x =>
                                                !globallyExcludedColumns.Contains(x.Key)
                                            )
                                            .Select(x => new ChangedRecord()
                                            {
                                                ColumnName = x.Key,
                                                OriginalValue = null,
                                                NewValue = x.Value?.ToString(),
                                            })
                                            .ToList();
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
                                                NewValue = change.NewValue?.ToString(),
                                            })
                                            .ToList();
                                    }

                                    List<ChangedRecord> globallyHandledRecords = new();

                                    if (
                                        changes.Any(x =>
                                            x.ColumnName == nameof(ISoftDelete.IsDeleted)
                                            && x.NewValue?.ToLower() == "true"
                                        )
                                    )
                                    {
                                        audit.Action = "Delete";
                                        var deletedRecord = changes.First(x =>
                                            x.ColumnName == nameof(ISoftDelete.IsDeleted)
                                        );
                                        changes.Remove(deletedRecord);
                                        deletedRecord.FriendlyName = "Deleted";
                                        deletedRecord.Message = "Successfully deleted.";
                                        globallyHandledRecords.Add(deletedRecord);
                                    }

                                    if (
                                        changes.Any(x =>
                                            x.ColumnName == nameof(ISoftDelete.DeletedAt)
                                            && !string.IsNullOrWhiteSpace(x.NewValue)
                                        )
                                    )
                                    {
                                        audit.Action = "Delete";
                                        var deletedRecord = changes.First(x =>
                                            x.ColumnName == nameof(ISoftDelete.DeletedAt)
                                        );
                                        changes.Remove(deletedRecord);
                                    }

                                    var processedRecords = ProcessChangedRecords.ProcessRecords(
                                        entry.EntityType.Name,
                                        changes
                                    );

                                    processedRecords.AddRange(globallyHandledRecords);

                                    if (!processedRecords.Any())
                                        return false;

                                    audit.ChangedProperties = JsonSerializer.Serialize(
                                        processedRecords
                                    );

                                    return true;
                                }
                            )
                    )
                    .IgnoreMatchedProperties(true)
            );
    }

    private static string ExtractUserId(
        string entityTypeName,
        IDictionary<string, object> columnValues
    )
    {
        return entityTypeName switch
        {
            nameof(User) => columnValues.First(x => x.Key == "Id").Value.ToString(),
            nameof(Player) => columnValues.First(x => x.Key == "UserId").Value.ToString(),
            _ => throw new InvalidOperationException($"Unsupported entity type: {entityTypeName}"),
        };
    }
}
