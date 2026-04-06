namespace ExpressedRealms.Powers.Reporting.powerCards.CardTypes;

public class WealthCardData : ICardData
{
    public int WealthLevel { get; set; }
    public double WealthIncome { get; set; }
    public bool IsDestitute { get; set; }
    public double BankedCash { get; set; }
    public double Liquadation { get; set; }
    public double InitialBasicItemIncome { get; set; }
}
