namespace ExpressedRealms.Admin.API.AdminCharacterList.GetCharacterList;

public class PrimaryCharacter
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Background { get; set; }
    public string Expression { get; set; } = null!;
    public required string PlayerName { get; set; }
    public int AssignedXp { get; set; }
    public int PlayerNumber { get; set; }
}
