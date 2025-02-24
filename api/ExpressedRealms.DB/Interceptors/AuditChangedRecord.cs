namespace ExpressedRealms.DB.Interceptors;

public class ChangedRecord
{
    public string ColumnName { get; set; }
    public string? OriginalValue { get; set; }
    public string? NewValue { get; set; }
    public string? FriendlyName { get; set; }
    public string? Message { get; set; }
}
