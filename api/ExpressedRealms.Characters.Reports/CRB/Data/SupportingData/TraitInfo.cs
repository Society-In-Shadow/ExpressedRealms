namespace ExpressedRealms.Characters.Reports.CRB.Data.SupportingData;

public class TraitInfo
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string LevelName { get; set; }
    public required string LevelDescription { get; set; }
    public string? UserNotes { get; set; }
    public bool IncludeInPrintOut { get; set; }
}
