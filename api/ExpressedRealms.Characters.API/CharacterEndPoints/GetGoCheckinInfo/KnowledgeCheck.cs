namespace ExpressedRealms.Characters.API.CharacterEndPoints.GetGoCheckinInfo;

public class KnowledgeCheck
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public bool IsDoctorateLevel { get; set; }
    public bool IsUnknownKnowledge { get; set; }
}
