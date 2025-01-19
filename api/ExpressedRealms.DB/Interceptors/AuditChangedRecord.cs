namespace ExpressedRealms.DB.Interceptors;

public record ChangedRecord(string ColumnName, string? OriginalValue, string? NewValue);
