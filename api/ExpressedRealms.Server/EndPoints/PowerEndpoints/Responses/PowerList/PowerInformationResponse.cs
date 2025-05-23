namespace ExpressedRealms.Server.EndPoints.PowerEndpoints.Responses.PowerList;

public class PowerInformationResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<DetailedInformation> Category { get; set; }
    public string Description { get; set; }
    public string GameMechanicEffect { get; set; }
    public string Limitation { get; set; }
    public DetailedInformation PowerDuration { get; set; }
    public DetailedInformation AreaOfEffect { get; set; }
    public DetailedInformation PowerLevel { get; set; }
    public DetailedInformation PowerActivationType { get; set; }
    public string Other { get; set; }
    public bool IsPowerUse { get; set; }
}
