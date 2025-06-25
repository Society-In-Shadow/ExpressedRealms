using ExpressedRealms.Characters.Repository.Stats.Enums;

namespace ExpressedRealms.Characters.API.CharacterEndPoints.StatDTOs;

internal record SingleStatInfo
{
    public SingleStatInfo(Characters.Repository.Stats.DTOs.SingleStatInfo dto)
    {
        Id = dto.Id;
        Name = dto.Name;
        Description = dto.Description;
        StatLevel = dto.StatLevel;
        AvailableXP = dto.AvailableXP;
        StatLevelInfo = new StatDetails()
        {
            Level = dto.StatLevelInfo.Level,
            Description = dto.StatLevelInfo.Description,
            TotalXP = dto.StatLevelInfo.TotalXP,
            Bonus = dto.StatLevelInfo.Bonus,
            XP = dto.StatLevelInfo.XP,
        };
    }

    /// <example>6</example>
    public StatType Id { get; set; }

    /// <example>Willpower</example>
    public string Name { get; set; }

    /// <example>Willpower (WIL) is the measure of your character’s mental toughness and tenacity, as well as the
    /// measure of the raw power of your character’s determination. </example>
    public string Description { get; set; }

    /// <example>7</example>
    public int StatLevel { get; set; }

    /// <example>128</example>
    public int AvailableXP { get; set; }

    public StatDetails StatLevelInfo { get; set; }
}
