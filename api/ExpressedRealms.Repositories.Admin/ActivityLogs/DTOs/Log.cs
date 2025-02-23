namespace ExpressedRealms.Repositories.Admin.DTOs;

public class Log
{
    public string Location { get; set; }
    public DateTime TimeStamp { get; set; }
    public string Action { get; set; }
    public string ChangedProperties { get; set; }
}
