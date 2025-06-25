using ExpressedRealms.Characters.Repository.Stats.Enums;

namespace ExpressedRealms.Characters.API.StatEndPoints.Requests;

internal class EditStatRequest
{
    /// <example>6</example>
    public int CharacterId { get; set; }

    /// <summary>This should be the same value as in the path.  Enum Values are : 1: </summary>
    /// <example>6</example>
    public StatType StatTypeId { get; set; }

    /// <summary>This is a value between 1 and 7, to represent the different levels.</summary>
    /// <example>7</example>
    public byte LevelTypeId { get; set; }
}
