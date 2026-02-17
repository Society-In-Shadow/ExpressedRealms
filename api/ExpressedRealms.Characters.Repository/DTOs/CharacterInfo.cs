namespace ExpressedRealms.Characters.Repository.DTOs;

public class CharacterInfo
{
    public required string CharacterName { get; set; }
    public required string Expression { get; set; }
    public required string PlayerName { get; set; }
    public int PlayerNumber { get; set; }
    public string? PrimaryProgressionName { get; set; }
    public string? SecondaryProgressionName { get; set; }
    public required string LookupId { get; set; }
}
