namespace ExpressedRealms.Expressions.Repository.Factions.Dtos;

public class FactionDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Background { get; set; }
    public int ExpressionId { get; set; }
    public List<FactionLevelListDto> Levels { get; set; } = new();
}