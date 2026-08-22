namespace ExpressedRealms.Characters.UseCases.Characters.GetGoCheckinInfo;

public class KnowledgeToCheck
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public bool IsDoctorateLevel { get; set; }
    public bool IsUnknownKnowledge { get; set; }
}