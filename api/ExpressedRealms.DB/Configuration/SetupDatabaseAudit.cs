using System.Text.Json;
using Audit.Core;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Expressions;

namespace ExpressedRealms.DB.Configuration;

public static class SetupDatabaseAudit
{
    public static void SetupAudit()
    {
        Audit
            .Core.Configuration.Setup()
            .UseEntityFramework(x =>
                x.AuditTypeExplicitMapper(m =>
                        m.Map<ExpressionSection, ExpressionSectionAuditTrail>(
                                (section, audit) =>
                                {
                                    audit.SectionId = section.Id;
                                    audit.ExpressionId = section.ExpressionId;
                                    return true;
                                }
                            )
                            .AuditEntityAction<IAuditTable>(
                                (evt, entry, audit) =>
                                {
                                    audit.Action = entry.Action;
                                    audit.Timestamp = DateTime.UtcNow;
                                    audit.UserId = evt.Environment.UserName;

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
                                            .ColumnValues.Select(x => new ChangedRecord(
                                                x.Key,
                                                null,
                                                x.Value?.ToString()
                                            ))
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
                                            .Select(x => new ChangedRecord(
                                                x.ColumnName,
                                                x.OriginalValue?.ToString(),
                                                x.NewValue?.ToString()
                                            ))
                                            .ToList();
                                    }

                                    audit.ChangedProperties = JsonSerializer.Serialize(changes);

                                    return true;
                                }
                            )
                    )
                    .IgnoreMatchedProperties(true)
            );
    }
}
