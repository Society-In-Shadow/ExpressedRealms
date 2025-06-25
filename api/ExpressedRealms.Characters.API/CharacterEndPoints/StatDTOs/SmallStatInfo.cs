using ExpressedRealms.Characters.Repository.Stats.Enums;

namespace ExpressedRealms.Characters.API.CharacterEndPoints.StatDTOs;

internal class SmallStatInfo(Characters.Repository.Stats.DTOs.SmallStatInfo smallStatInfo)
{
    /// <example>WIL</example>
    public string ShortName { get; set; } = smallStatInfo.ShortName;

    /// <example>1</example>
    public int Level { get; set; } = smallStatInfo.Level;

    /// <example>-1</example>
    public int Bonus { get; set; } = smallStatInfo.Bonus;

    /// <example>6</example>
    public StatType StatTypeId { get; set; } = smallStatInfo.StatTypeId;
}
