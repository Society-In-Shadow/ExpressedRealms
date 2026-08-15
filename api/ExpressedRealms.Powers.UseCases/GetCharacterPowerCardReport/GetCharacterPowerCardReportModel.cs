using ExpressedRealms.Powers.Reporting.powerCards.CardPluginSystem;

namespace ExpressedRealms.Powers.UseCases.GetCharacterPowerCardReport;

public class GetCharacterPowerCardReportModel
{
    public int CharacterId { get; set; }
    public bool IsFiveByThree { get; set; }
    public List<ICardTile> CardTiles { get; set; } = [];
}
