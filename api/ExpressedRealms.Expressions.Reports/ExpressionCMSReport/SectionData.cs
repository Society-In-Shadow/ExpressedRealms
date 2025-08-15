namespace ExpressedRealms.Expressions.Reports.ExpressionCMSReport;

public class SectionData
{
    public required string Name { get; set; }
    public required string Content { get; set; }
    public int SortOrder { get; set; }
    public int Level { get; set; }
    public int SectionTypeId { get; set; }
    public List<KnowledgeData>? Knowledges { get; set; }
    public List<BlessingData>? Blessings { get; set; }
}
