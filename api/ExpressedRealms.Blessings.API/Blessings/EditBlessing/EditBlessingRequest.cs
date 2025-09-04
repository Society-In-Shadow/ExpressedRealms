namespace ExpressedRealms.Blessings.API.Blessings.EditBlessing;

public class EditBlessingRequest
{
    public required string Name { get; set; } = null!;
    public required string Description { get; set; }
    public required string Category { get; set; }
    public required string Type { get; set; }
}
