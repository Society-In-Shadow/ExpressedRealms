namespace ExpressedRealms.Knowledges.UseCases.KnowledgeSpecializations.CreateSpecialization;

public class CreateSpecializationModel
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public int KnowledgeMappingId { get; set; }
    public string? Notes { get; set; }
}
