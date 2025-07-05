namespace ExpressedRealms.Expressions.Repository.ExpressionTextSections.DTOs;

public class PotentialParentsDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public List<PotentialParentsDto> SubSections { get; set; } = new();
}
