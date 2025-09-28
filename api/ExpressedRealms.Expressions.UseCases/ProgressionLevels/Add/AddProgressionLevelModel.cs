namespace ExpressedRealms.Expressions.UseCases.ProgressionLevels.Add;

public class AddProgressionLevelModel
{ 
    public int ProgressionId { get; set; }
    public int XlLevel { get; set; }
    public required string Description { get; set; }
}
