namespace ExpressedRealms.Knowledges.UseCases.KnowledgeLevels;

public class KnowledgeLevelModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int Level { get; set; }
    public int SpecializationCount { get; set; }
    public int StoneModifier { get; set; }
    public int GeneralXpCost { get; set; }
    public int TotalGeneralXpCost { get; set; }
    public int UnknownXpCost { get; set; }
    public int TotalUnknownXpCost { get; set; }
}
