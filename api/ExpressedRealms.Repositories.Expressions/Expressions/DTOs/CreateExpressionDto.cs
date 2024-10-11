namespace ExpressedRealms.Repositories.Expressions.Expressions.DTOs;

public record CreateExpressionDto
{
    public string Name { get; set; }
    public string ShortDescription { get; set; }
    public string NavMenuImage { get; set; }
}