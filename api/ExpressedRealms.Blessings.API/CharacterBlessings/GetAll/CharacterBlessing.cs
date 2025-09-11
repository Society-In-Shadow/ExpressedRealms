namespace ExpressedRealms.Blessings.API.CharacterBlessings.GetAll;

public class CharacterBlessing
{
    public int BlessingId { get; set; }
    public int BlessingLevelId { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string LevelName { get; set; }
    public required string LevelDescription { get; set; }
}
