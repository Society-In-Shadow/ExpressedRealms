namespace ExpressedRealms.Server.EndPoints.CharacterEndPoints.StatDTOs;

public class EditStatDTO
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
