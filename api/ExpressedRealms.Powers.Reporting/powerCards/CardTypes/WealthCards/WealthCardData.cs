namespace ExpressedRealms.Powers.Reporting.powerCards.CardTypes.WealthCards;

public class WealthCardData : ICardData
{
    public int WealthLevel { get; set; }
    public double InitialBasicItemIncome { get; set; }
    public string CharacterName { get; set; }
    public List<KeyValuePair<string, string>> AppliedBlessings { get; set; } = [];
    public List<WealthTableLine> WealthTableLines { get; set; }
}
