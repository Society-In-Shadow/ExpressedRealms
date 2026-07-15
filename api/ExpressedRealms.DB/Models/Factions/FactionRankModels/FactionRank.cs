using ExpressedRealms.DB.Models.Factions.CharacterFactionMappingModels;
using ExpressedRealms.DB.Models.Factions.FactionLevelModels;

namespace ExpressedRealms.DB.Models.Factions.FactionRankModels;

public class FactionRank
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public virtual ICollection<FactionLevel> FactionLevels { get; set; } =
        new HashSet<FactionLevel>();
    public virtual ICollection<CharacterFactionMapping> CharacterFactionMappings { get; set; } =
        new HashSet<CharacterFactionMapping>();
}
