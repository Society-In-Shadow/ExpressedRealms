namespace ExpressedRealms.Characters.Reports.CRB.DataCards.AdDisadCards;

public class BlessingInfo
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string LevelName { get; set; }
    public required string LevelDescription { get; set; }
    public string? UserNotes { get; set; }
    public required string BlessingType { get; set; }
}