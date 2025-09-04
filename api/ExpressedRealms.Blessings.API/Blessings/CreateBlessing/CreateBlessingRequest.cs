namespace ExpressedRealms.Blessings.API.Blessings.CreateBlessing;

public class CreateBlessingRequest
{
    public required string Name { get; set; } = null!;
    public required string Description { get; set; }
    public string Category { get; set; }
    public string Type { get; set; }
}
