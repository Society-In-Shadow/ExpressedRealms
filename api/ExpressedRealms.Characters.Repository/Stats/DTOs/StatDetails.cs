namespace ExpressedRealms.Characters.Repository.Stats.DTOs;

public sealed record StatDetails
{
    /// <example>4</example>
    public byte Level { get; set; }

    /// <example>+1</example>
    public int Bonus { get; set; }

    /// <example>12</example>
    public int XP { get; set; }

    /// <example>24</example>
    public int TotalXP { get; set; }

    /// <example>The character with a WIL of 7 is the individual who laughs at every horror movie, wins a posturing
    /// contest with a silverback gorilla, or intentionally waits to finish disarming the bomb till the timer reaches
    /// 1 second.</example>
    public string Description { get; set; } = null!;
}
