namespace ExpressedRealms.Knowledges.UseCases.CharacterKnowledgeMappings.GetEdit;

public class SpecializationReturnModel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public string? Notes { get; set; }

    /// <summary>
    /// This should block the ability to edit the name and delete the specialization
    /// </summary>
    public bool BlockFactionChanges { get; set; }
}
