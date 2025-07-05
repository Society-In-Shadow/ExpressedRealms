namespace ExpressedRealms.Expressions.Repository.ExpressionTextSections.DTOs;

public record ExpressionTextSectionDto
{
    public string Name { get; set; } = null!;
    public string ShortDescription { get; set; } = null!;
    public string NavMenuImage { get; set; } = null!;
}
