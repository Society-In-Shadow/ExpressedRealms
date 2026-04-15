namespace ExpressedRealms.Powers.Reporting.powerCards.CardTypes.WealthCards;

public class WealthCardData : ICardData
{
    public int WealthLevel { get; set; }
    public double WealthIncome { get; set; }
    public double BankedCash { get; set; }
    public double Liquadation { get; set; }
    public double InitialBasicItemIncome { get; set; }
    public string CharacterName { get; set; }
    public List<WealthTableLine> WealthTableLines { get; set; }
}
