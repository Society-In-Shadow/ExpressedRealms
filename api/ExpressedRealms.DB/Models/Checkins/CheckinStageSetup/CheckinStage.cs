using ExpressedRealms.DB.Models.Checkins.CheckinStageMappingSetup;

namespace ExpressedRealms.DB.Models.Checkins.CheckinStageSetup;

public class CheckinStage
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }

    public virtual ICollection<CheckinStageMapping> CheckinStageMappings { get; set; } = new HashSet<CheckinStageMapping>();
}
