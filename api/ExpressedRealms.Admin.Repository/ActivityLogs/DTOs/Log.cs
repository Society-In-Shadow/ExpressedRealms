namespace ExpressedRealms.Admin.Repository.ActivityLogs.DTOs;

public class Log
{
    public required string Location { get; set; }
    public DateTime TimeStamp { get; set; }
    public required string Action { get; set; }
    public required string ChangedProperties { get; set; }
}
