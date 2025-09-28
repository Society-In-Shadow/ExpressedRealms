namespace ExpressedRealms.Expressions.UseCases.ProgressionLevels.Edit;

public class EditProgressionLevelModel
{
    public int ProgressionPathId { get; set; }
    public int ProgressionLevelId { get; set; }
    public int XlLevel { get; set; }
    public required string Description { get; set; }

}
