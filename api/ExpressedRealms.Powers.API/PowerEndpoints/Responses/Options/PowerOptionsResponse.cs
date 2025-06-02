namespace ExpressedRealms.Powers.API.PowerEndpoints.Responses.Options;

public class PowerOptionsResponse
{
    public List<DetailedEditInformation> Category { get; set; }
    public List<DetailedEditInformation> PowerDuration { get; set; }
    public List<DetailedEditInformation> AreaOfEffect { get; set; }
    public List<DetailedEditInformation> PowerLevel { get; set; }
    public List<DetailedEditInformation> PowerActivationType { get; set; }
}
