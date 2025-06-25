namespace ExpressedRealms.Characters.API.CharacterEndPoints.Responses;

internal class BenefitItemResponse
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public byte Modifier { get; set; }
    public byte LevelId { get; set; }
}
