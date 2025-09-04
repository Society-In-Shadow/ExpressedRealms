namespace ExpressedRealms.Blessings.API.Blessings.EditBlessing;

public class EditBlessingRequest
{
    public required string Name { get; set; } = null!;
    public required string Description { get; set; }
    public string Category { get; set; }
    public string Type { get; set; }
    public int Id { get; set; }
}
