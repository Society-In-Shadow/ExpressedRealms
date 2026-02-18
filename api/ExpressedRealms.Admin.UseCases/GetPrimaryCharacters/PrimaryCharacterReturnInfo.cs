namespace ExpressedRealms.Admin.UseCases.GetPrimaryCharacters;

public class PrimaryCharacterReturnInfo
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Expression { get; set; } = null!;
    public required string PlayerName { get; set; }
    public int PlayerNumber { get; set; }
    public int? PlayerStageId { get; set; }
}
