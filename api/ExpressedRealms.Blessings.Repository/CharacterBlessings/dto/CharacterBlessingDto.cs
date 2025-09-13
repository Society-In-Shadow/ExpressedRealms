namespace ExpressedRealms.Blessings.Repository.CharacterBlessings.dto;

public class CharacterBlessingDto
{
    public int BlessingId { get; set; }
    public int BlessingLevelId { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string LevelName { get; set; }
    public required string LevelDescription { get; set; }
    public string? Notes { get; set; }
    public int Id { get; set; }
    public required string Type { get; set; }
    public required string SubCategory { get; set; }
}
