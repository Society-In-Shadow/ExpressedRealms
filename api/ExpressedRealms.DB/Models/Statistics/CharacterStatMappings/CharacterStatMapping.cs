using ExpressedRealms.DB.Models.Characters;

namespace ExpressedRealms.DB.Models.Statistics.CharacterStatMappings;

public class CharacterStatMapping
{
    public int Id { get; set; }
    public int CharacterId { get; set; }
    public byte StatTypeId { get; set; }
    public byte StatLevelId { get; set; }

    public virtual StatLevel StatLevel { get; set; } = null!;
    public virtual Character Character { get; set; } = null!;
    public virtual StatType StatType { get; set; } = null!;
}
