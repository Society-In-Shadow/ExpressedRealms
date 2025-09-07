using ExpressedRealms.Characters.Repository.Stats.Enums;

namespace ExpressedRealms.Characters.API.StatEndPoints.Responses;

internal class SmallStatInfo(Repository.Stats.DTOs.SmallStatInfo smallStatInfo)
{
    /// <example>WIL</example>
    public string ShortName { get; set; } = smallStatInfo.ShortName;

    /// <example>Willpower</example>
    public string Name { get; set; } = smallStatInfo.Name;

    /// <example>1</example>
    public int Level { get; set; } = smallStatInfo.Level;

    /// <example>-1</example>
    public int Bonus { get; set; } = smallStatInfo.Bonus;

    /// <example>6</example>
    public StatType StatTypeId { get; set; } = smallStatInfo.StatTypeId;
}
