namespace ExpressedRealms.Expressions.API.ExpressionSubSectionEndpoints.DTOs;

public class AvailableParentsDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public List<AvailableParentsDto> SubSections { get; set; } = new();
}
