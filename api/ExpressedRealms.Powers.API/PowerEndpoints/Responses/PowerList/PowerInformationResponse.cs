using ExpressedRealms.Powers.Repository.Powers.DTOs.PowerList;

namespace ExpressedRealms.Powers.API.PowerEndpoints.Responses.PowerList;

public class PowerInformationResponse
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required List<DetailedInformation> Category { get; set; }
    public required string Description { get; set; }
    public required string GameMechanicEffect { get; set; }
    public string? Limitation { get; set; }
    public required DetailedInformation PowerDuration { get; set; }
    public required DetailedInformation AreaOfEffect { get; set; }
    public required DetailedInformation PowerLevel { get; set; }
    public required DetailedInformation PowerActivationType { get; set; }
    public string? Other { get; set; }
    public bool IsPowerUse { get; set; }
    public string? Cost { get; set; }
    public PrerequisiteDetails? Prerequisites { get; set; }
}
