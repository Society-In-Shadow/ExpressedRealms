namespace ExpressedRealms.Server.EndPoints.CharacterEndPoints.Responses;

public class CharacterSkillOptionsResponse
{
    public int SkillTypeId { get; set; }
    public string Name { get; set; } = null!;
    public object Description { get; set; } = null!;
    public int LevelId { get; set; }

    public List<BenefitItemResponse> Benefits { get; set; } = new();
    public int ExperienceCost { get; set; }
}
