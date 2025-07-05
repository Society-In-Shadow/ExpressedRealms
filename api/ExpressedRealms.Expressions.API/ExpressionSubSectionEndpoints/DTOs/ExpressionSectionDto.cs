namespace ExpressedRealms.Expressions.API.ExpressionSubSectionEndpoints.DTOs;

public class ExpressionSectionDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Content { get; set; }
    public List<ExpressionSectionDto> SubSections { get; set; } = new();
}
