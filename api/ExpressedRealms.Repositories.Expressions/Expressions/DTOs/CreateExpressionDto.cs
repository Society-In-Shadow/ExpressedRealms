namespace ExpressedRealms.Repositories.Expressions.Expressions.DTOs;

public record CreateExpressionDto
{
    public string Name { get; set; } = null!;
    public string ShortDescription { get; set; } = null!;
    public string NavMenuImage { get; set; } = null!;
}
