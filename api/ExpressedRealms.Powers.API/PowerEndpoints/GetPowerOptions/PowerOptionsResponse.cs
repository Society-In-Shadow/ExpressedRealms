namespace ExpressedRealms.Powers.API.PowerEndpoints.GetPowerOptions;

public class PowerOptionsResponse
{
    public List<DetailedEditInformation> Category { get; set; } = new();
    public List<DetailedEditInformation> PowerDuration { get; set; } = new();
    public List<DetailedEditInformation> AreaOfEffect { get; set; } = new();
    public List<DetailedEditInformation> PowerLevel { get; set; } = new();
    public List<DetailedEditInformation> PowerActivationType { get; set; } = new();
}
