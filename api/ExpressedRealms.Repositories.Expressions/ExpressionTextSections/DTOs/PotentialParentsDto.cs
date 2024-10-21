namespace ExpressedRealms.Repositories.Expressions.ExpressionTextSections.DTOs;

public class PotentialParentsDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<PotentialParentsDto> SubSections { get; set; }
}
