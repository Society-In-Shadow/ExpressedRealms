namespace ExpressedRealms.Repositories.Expressions.Expressions.DTOs;

public class ExpressionNavigationMenuItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ShortDescription { get; set; } = null!;
    public string NavMenuImage { get; set; } = null!;
    public string PublishStatusName { get; set; }
    public PublishTypes PublishStatusId { get; set; }
}