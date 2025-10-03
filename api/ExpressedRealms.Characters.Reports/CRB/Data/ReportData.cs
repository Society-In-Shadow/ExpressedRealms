using ExpressedRealms.Characters.Reports.CRB.Data.SupportingData;

namespace ExpressedRealms.Characters.Reports.CRB.Data;

public class ReportData
{
    public Traits Traits { get; set; }
    public BasicInfo BasicInfo { get; set; }
    public Skills Skills { get; set; }
    public List<PowerInfo> Powers { get; set; }
    public List<KnowledgeInfo> Knowledges { get; set; }
}