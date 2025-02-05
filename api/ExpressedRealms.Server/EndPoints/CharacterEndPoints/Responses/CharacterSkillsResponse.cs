namespace ExpressedRealms.Server.EndPoints.CharacterEndPoints.Responses;

public class CharacterSkillsResponse
{
    public byte SkillTypeId { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public byte LevelId { get; set; }
    public string LevelName { get; set; } = null!;
    public string LevelDescription { get; set; } = null!;
    public byte SkillSubTypeId { get; set; }
    public List<BenefitItemResponse> Benefits { get; set; } = new();
    public int XP { get; set; }
}
