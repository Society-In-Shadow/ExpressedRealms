using ExpressedRealms.Repositories.Characters.Stats.Enums;

namespace ExpressedRealms.Repositories.Characters.Stats.DTOs;

public class SmallStatInfo
{
    /// <example>WIL</example>
    public string ShortName { get; set; }

    /// <example>1</example>
    public int Level { get; set; }

    /// <example>-1</example>
    public int Bonus { get; set; }

    /// <example>6</example>
    public StatType StatTypeId { get; set; }
}
