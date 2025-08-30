namespace ExpressedRealms.Blessings.UseCases.Blessings.EditBlessings;

public class EditBlessingModel
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string Type { get; set; }
    public required string SubCategory { get; set; }
    public int Id { get; set; }
}
