namespace ExpressedRealms.DB.Exceptions;

public class MissingAuditColumnException : Exception
{
    public string ColumnName { get; }

    public MissingAuditColumnException(string columnName)
        : base($"Missing audit column handler for {columnName}")
    {
        ColumnName = columnName;
    }
}
