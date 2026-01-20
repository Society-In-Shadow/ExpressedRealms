namespace ExpressedRealms.Admin.API.AdminEndpoints.ViewActivityLogs;

public class LogDto
{
    public int Id { get; set; }
    public required string Location { get; set; }
    public DateTime TimeStamp { get; set; }
    public required string Action { get; set; }
    public required string ChangedProperties { get; set; }
}
