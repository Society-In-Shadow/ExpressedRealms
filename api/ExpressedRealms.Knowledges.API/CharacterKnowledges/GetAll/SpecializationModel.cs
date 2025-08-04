namespace ExpressedRealms.Knowledges.API.CharacterKnowledges.GetAll;

public class SpecializationModel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public string? Notes { get; set; }
}
