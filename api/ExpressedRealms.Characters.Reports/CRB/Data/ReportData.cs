using ExpressedRealms.Characters.Reports.CRB.Data.SupportingData;

namespace ExpressedRealms.Characters.Reports.CRB.Data;

public class ReportData
{
    public Traits Traits { get; set; } = new();
    public BasicInfo BasicInfo { get; set; }
    public SkillInfo SkillInfo { get; set; } = new();
    public List<PowerInfo> Powers { get; set; } = new();
    public List<KnowledgeInfo> Knowledges { get; set; } = new();
    public List<ContactInfo> Contacts { get; set; } = new();
    public ProficiencyData ProficiencyInfo { get; set; }
    public StatModifierInfo StatInfo { get; set; }
}
