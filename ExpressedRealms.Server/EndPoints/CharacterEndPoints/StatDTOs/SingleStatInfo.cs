namespace ExpressedRealms.Server.EndPoints.CharacterEndPoints.StatDTOs;

public class SingleStatInfo
{
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
