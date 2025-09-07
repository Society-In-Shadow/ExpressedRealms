using ExpressedRealms.Characters.Repository.Stats.Enums;

namespace ExpressedRealms.Characters.Repository.Stats.DTOs;

public class SmallStatInfo
{
    /// <example>WIL</example>
    public required string ShortName { get; set; }

    /// <example>1</example>
    public int Level { get; set; }

    /// <example>-1</example>
    public int Bonus { get; set; }

    /// <example>6</example>
    public StatType StatTypeId { get; set; }

    public required string Name { get; set; }
}
