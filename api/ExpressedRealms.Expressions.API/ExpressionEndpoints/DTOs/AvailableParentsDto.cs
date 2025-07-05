namespace ExpressedRealms.Expressions.API.ExpressionEndpoints.DTOs;

public class AvailableParentsDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<AvailableParentsDto> SubSections { get; set; } = new();
}
