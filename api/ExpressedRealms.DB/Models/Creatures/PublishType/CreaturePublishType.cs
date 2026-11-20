using ExpressedRealms.DB.Models.Creatures.CreatureSetup;

namespace ExpressedRealms.DB.Models.Creatures.PublishType;

public class CreaturePublishType
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    public virtual List<Creature> Creatures { get; set; } = null!;
}
