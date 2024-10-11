namespace ExpressedRealms.Repositories.Expressions.Expressions.DTOs;

public record EditExpressionDto
{
    public int Id { get; init; }
    public string Name { get; set; }
    public string ShortDescription { get; set; }
    public string NavMenuImage { get; set; }
}