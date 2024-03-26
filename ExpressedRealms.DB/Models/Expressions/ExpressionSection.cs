namespace ExpressedRealms.DB.Models.Expressions;

public class ExpressionSection
{
    public int Id { get; set; }
    public int ExpressionId { get; set; }
    public int SectionTypeId { get; set; }
    public int? ParentId { get; set; }
    public string Name { get; set; } = null!;
    public string Content { get; set; } = null!;

    public virtual Expression Expression { get; set; } = null!;
    public virtual ExpressionSection? Parent { get; set; }
    public virtual ExpressionSectionType SectionType { get; set; } = null;
    public virtual List<ExpressionSection>? Children { get; set; }
}
