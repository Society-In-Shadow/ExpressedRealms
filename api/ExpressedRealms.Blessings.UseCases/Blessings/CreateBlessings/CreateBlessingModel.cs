namespace ExpressedRealms.Blessings.UseCases.Blessings.CreateBlessings;

public class CreateBlessingModel
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string Type { get; set; }
    public required string SubCategory { get; set; }
}
