namespace ExpressedRealms.Events.API.Repositories.EventCheckin.Dtos;

public class CharacterStorageOptin
{
    public int Id { get; set; }
    public required string ApproverName { get; set; }
    public required string PlayerName { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    public int Amount { get; set; }
}