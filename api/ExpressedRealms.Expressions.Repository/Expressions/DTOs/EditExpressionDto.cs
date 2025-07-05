namespace ExpressedRealms.Expressions.Repository.Expressions.DTOs;

public record EditExpressionDto
{
    public int Id { get; init; }
    public string Name { get; set; } = null!;
    public string ShortDescription { get; set; } = null!;
    public string NavMenuImage { get; set; } = null!;
    public PublishTypes PublishStatus { get; set; }
}
