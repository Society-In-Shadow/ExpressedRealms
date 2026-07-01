namespace ExpressedRealms.Expressions.UseCases.FactionLevelUseCases.CreateFactionLevel;

public class CreateFactionLevelModel
{
    public required int FactionId { get; set; }
    public int KnowledgeId { get; set; }
    public required string Specialization { get; set; }
}
