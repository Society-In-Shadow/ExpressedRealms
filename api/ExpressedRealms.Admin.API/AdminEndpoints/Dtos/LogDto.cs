namespace ExpressedRealms.Server.EndPoints.AdminEndpoints.Dtos;

public class LogDto
{
    public int Id { get; set; }
    public string Location { get; set; }
    public DateTime TimeStamp { get; set; }
    public string Action { get; set; }
    public string ChangedProperties { get; set; }
}
