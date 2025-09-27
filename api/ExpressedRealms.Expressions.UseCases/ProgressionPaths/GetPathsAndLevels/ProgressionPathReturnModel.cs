namespace ExpressedRealms.Expressions.UseCases.ProgressionPaths.GetPathsAndLevels;

public class ProgressionPathReturnModel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required List<ProgressionLevelReturnModel> Levels { get; set; }
}
