namespace ExpressedRealms.Admin.API.ArchetypeEndpoints.GetArchetypes;

public class Archetype
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required string ExpressionName { get; set; }
}
