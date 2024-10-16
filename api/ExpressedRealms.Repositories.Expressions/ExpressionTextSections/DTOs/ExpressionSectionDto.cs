namespace ExpressedRealms.Repositories.Expressions.ExpressionTextSections.DTOs;

public class ExpressionSectionDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Content { get; set; }
    public List<ExpressionSectionDto> SubSections { get; set; } = new();
}
