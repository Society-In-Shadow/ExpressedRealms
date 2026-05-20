using ExpressedRealms.DB.Models.Statistics.CharacterStatMappings;

namespace ExpressedRealms.DB.Models.Statistics;

public class StatLevel
{
    public byte Id { get; set; }
    public int Bonus { get; set; }
    public int XPCost { get; set; }
    public int TotalXPCost { get; set; }

    public virtual List<StatDescriptionMapping> StatDescriptionMappings { get; set; } = null!;
    public virtual ICollection<CharacterStatMapping> CharacterStatMappings { get; set; }  = new HashSet<CharacterStatMapping>();
}
