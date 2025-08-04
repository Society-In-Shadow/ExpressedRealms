namespace ExpressedRealms.Knowledges.UseCases.KnowledgeSpecializations.EditSpecialization;

public class EditSpecializationModel
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public int Id { get; set; }
    public string? Notes { get; set; }
}
