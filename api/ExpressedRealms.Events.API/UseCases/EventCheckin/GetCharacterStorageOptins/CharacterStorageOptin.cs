namespace ExpressedRealms.Events.API.UseCases.EventCheckin.GetCharacterStorageOptins;

public class CharacterStorageOptin
{
    public int Id { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    public required string ApproverName { get; set; }
    public required string PlayerName { get; set; }
    public int Amount { get; set; }
}
