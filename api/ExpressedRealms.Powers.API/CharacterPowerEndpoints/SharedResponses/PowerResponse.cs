using ExpressedRealms.Powers.API.CharacterPowerEndpoints.GetAll.Responses;

namespace ExpressedRealms.Powers.UseCases.CharacterPower.GetPickablePowers.ReturnModels;

public class PowerResponse
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public List<DetailedInformationResponse>? Category { get; set; }
    public required string Description { get; set; }
    public required string GameMechanicEffect { get; set; }
    public string? Limitation { get; set; }
    public required DetailedInformationResponse PowerDuration { get; set; }
    public required DetailedInformationResponse AreaOfEffect { get; set; }
    public required DetailedInformationResponse PowerLevel { get; set; }
    public required DetailedInformationResponse PowerActivationType { get; set; }
    public string? Other { get; set; }
    public bool IsPowerUse { get; set; }
    public string? Cost { get; set; }
    public int SortOrder { get; set; }
    public PrerequisiteDetailsResponse? Prerequisites { get; set; }
}
