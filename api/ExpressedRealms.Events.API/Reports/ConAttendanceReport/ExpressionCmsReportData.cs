namespace ExpressedRealms.Events.API.Reports.ConAttendanceReport;

public class ExpressionCmsReportData
{
    public List<string> Attendees { get; set; } = new();
    public required string ConName { get; set; }
}
