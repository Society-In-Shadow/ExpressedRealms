namespace ExpressedRealms.DB.Models.Expressions;

public class ExpressionSectionType
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public virtual List<ExpressionSection>? ExpressionSections { get; set; }
}
