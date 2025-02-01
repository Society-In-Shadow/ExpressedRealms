namespace ExpressedRealms.Server.EndPoints.CharacterEndPoints.Responses;

public class BenefitItemResponse
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public byte Modifier { get; set; }
    public byte LevelId { get; set; }
}
