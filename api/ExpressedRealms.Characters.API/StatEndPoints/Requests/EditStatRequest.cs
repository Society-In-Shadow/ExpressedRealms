namespace ExpressedRealms.Characters.API.StatEndPoints.Requests;

internal class EditStatRequest
{
    /// <summary>This is a value between 1 and 7, to represent the different levels.</summary>
    /// <example>7</example>
    public byte LevelTypeId { get; set; }
}
