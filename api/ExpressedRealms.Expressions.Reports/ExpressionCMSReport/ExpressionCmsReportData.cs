namespace ExpressedRealms.Expressions.Reports.ExpressionCMSReport;

public class ExpressionCmsReportData
{
    public required string ExpressionName { get; set; }
    public List<SectionData> Sections { get; set; } = new();
}
