namespace ExpressedRealms.Powers.Reporting.powerCards;

public class DataCard
{
    public CardTypeEnum CardType { get; set; }
    public required ICardData CardData { get; set; }
}