namespace ExpressedRealms.Powers.Repository.PowerPaths.DTOs.PowerPathCreate;

public class CreatePowerPathModel
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public int ExpressionId { get; set; }
}
