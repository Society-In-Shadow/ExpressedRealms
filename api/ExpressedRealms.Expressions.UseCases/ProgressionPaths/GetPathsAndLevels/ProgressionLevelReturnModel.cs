namespace ExpressedRealms.Expressions.UseCases.ProgressionPaths.GetPathsAndLevels;

public class ProgressionLevelReturnModel
{
    public int Id { get; set; }
    public required string Description { get; set; }
    public int XlLevel { get; set; }
    public int? ModifierGroupId { get; set; }
}
