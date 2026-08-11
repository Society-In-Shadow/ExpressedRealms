using QuestPDF.Fluent;

namespace ExpressedRealms.Powers.Reporting.powerCards.CardPluginSystem;

public interface ICardTile
{
    void Populate(ColumnDescriptor col);
}
