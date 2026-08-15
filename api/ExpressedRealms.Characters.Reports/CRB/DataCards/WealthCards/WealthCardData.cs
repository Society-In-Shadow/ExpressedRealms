namespace ExpressedRealms.Characters.Reports.CRB.DataCards.WealthCards;

public class WealthCardData
{
    public int WealthLevel { get; set; }
    public double InitialBasicItemIncome { get; set; }
    public List<KeyValuePair<string, string>> AppliedBlessings { get; set; } = [];
    public List<WealthTableLine> WealthTableLines { get; set; } = [];
}
