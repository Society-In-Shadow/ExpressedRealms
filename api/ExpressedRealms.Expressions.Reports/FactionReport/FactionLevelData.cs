using ExpressedRealms.Powers.Reporting.PowerBookletFormat;

namespace ExpressedRealms.Expressions.Reports.FactionReport;

public class FactionLevelData
{
    public required string RankName { get; set; }
    public string? KnowledgeName { get; set; }
    public string? KnowledgeLevel { get; set; }
    public string? KnowledgeSpecialization { get; set; }
    public PowerData? Power { get; set; }
}
