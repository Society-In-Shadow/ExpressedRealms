namespace ExpressedRealms.Admin.UseCases.Archetypes.GetArchetypes;

public class ArchetypeModel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string ExpressionName { get; set; }
    public string? Description { get; set; }
}
