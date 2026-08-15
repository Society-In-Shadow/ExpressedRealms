using ExpressedRealms.Characters.Reports.CRB.DataCards.WealthCards;

namespace ExpressedRealms.Characters.Reports.CRB.Data.SupportingData;

public class WealthInfoDto
{
    public required string CharacterName { get; set; }
    public int WealthLevel { get; set; }
    public double WealthIncome { get; set; }
    public double InitialBasicItemIncome { get; set; }
    public List<KeyValuePair<string, string>> AppliedBlessings { get; set; } = [];
    public List<WealthTableLine> WealthTableLines { get; set; } = [];
}
