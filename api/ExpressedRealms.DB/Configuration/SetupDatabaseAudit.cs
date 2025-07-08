using System.Text.Json;
using Audit.Core;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Expressions.ExpressionSectionSetup;
using ExpressedRealms.DB.Models.Expressions.ExpressionSetup;
using ExpressedRealms.DB.Models.Knowledges.KnowledgeModels.Audit;
using ExpressedRealms.DB.Models.Powers.PowerPathSetup;
using ExpressedRealms.DB.Models.Powers.PowerSetup.Audit;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.PlayerSetup;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserRoles;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;

namespace ExpressedRealms.DB.Configuration;

public static class SetupDatabaseAudit
{
    public static void SetupAudit()
    {
        var globallyExcludedColumns = new List<string>()
        {
            "Id",
            "id",
            nameof(ISoftDelete.IsDeleted),
            "is_deleted",
            nameof(ISoftDelete.DeletedAt),
            "deleted_at",
        };
        Audit
            .Core.Configuration.Setup()
            .UseEntityFramework(x =>
                x.AuditTypeExplicitMapper(m =>
                        m.AddExpressionSectionAuditTrailMapping()
                            .AddExpressionAuditTrailMapping()
                            .AddUserAuditTrailMapping()
                            .AddPlayerAuditTrailMapping()
                            .AddUserRoleAuditTrailMapping()
                            .AddPowerPathAuditTrailMapping()
                            .AddPowerAuditTrailMapping()
                            .AddKnowledgeAuditTrailMapping()
                            .AuditEntityAction<IAuditTable>(
                                (evt, entry, audit) =>
                                {
                                    audit.Action = entry.Action;
                                    audit.Timestamp = DateTime.UtcNow;

                                    // Need to handle edge case of a user being created
                                    if (!evt.CustomFields.ContainsKey("UserId"))
                                    {
                                        audit.ActorUserId = ExtractUserId(
                                            entry.EntityType.Name,
                                            entry.ColumnValues
                                        );
                                    }
                                    else
                                    {
                                        audit.ActorUserId = evt.CustomFields["UserId"]?.ToString();
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
                                    else if (
                                        string.Compare(
                                            audit.Action,
                                            "delete",
                                            StringComparison.InvariantCultureIgnoreCase
                                        ) == 0
                                    )
                                    {
                                        audit.ChangedProperties = JsonSerializer.Serialize(
                                            new List<ChangedRecord>()
                                            {
                                                new ChangedRecord()
                                                {
                                                    Message =
                                                        "Item was permanently deleted / removed.",
                                                    FriendlyName = "Deleted",
                                                },
                                            }
                                        );
                                        return true;
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
                                            (
                                                x.ColumnName == nameof(ISoftDelete.IsDeleted)
                                                || x.ColumnName == "is_deleted"
                                            )
                                            && x.NewValue?.ToLower() == "true"
                                        )
                                    )
                                    {
                                        audit.Action = "Delete";
                                        var deletedRecord = changes.First(x =>
                                            (
                                                x.ColumnName == nameof(ISoftDelete.IsDeleted)
                                                || x.ColumnName == "is_deleted"
                                            )
                                        );
                                        changes.Remove(deletedRecord);
                                        deletedRecord.FriendlyName = "Deleted";
                                        deletedRecord.Message = "Successfully deleted.";
                                        globallyHandledRecords.Add(deletedRecord);
                                    }

                                    if (
                                        changes.Any(x =>
                                            (
                                                x.ColumnName == nameof(ISoftDelete.DeletedAt)
                                                || x.ColumnName == "deleted_at"
                                            ) && !string.IsNullOrWhiteSpace(x.NewValue)
                                        )
                                    )
                                    {
                                        audit.Action = "Delete";
                                        var deletedRecord = changes.First(x =>
                                            (
                                                x.ColumnName == nameof(ISoftDelete.DeletedAt)
                                                || x.ColumnName == "deleted_at"
                                            )
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
