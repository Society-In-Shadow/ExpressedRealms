using ExpressedRealms.Repositories.Characters.Stats.Enums;

namespace ExpressedRealms.Server.EndPoints.CharacterEndPoints.StatDTOs;

public class SmallStatInfo(Repositories.Characters.Stats.DTOs.SmallStatInfo smallStatInfo)
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
