namespace ExpressedRealms.Powers.Reporting.powerCards.CardTypes.PowerCards;

public class PowerReportData
{
    public required string CharacterName { get; set; }
    public List<PowerCardData> PowerCards { get; set; } = [];
}
