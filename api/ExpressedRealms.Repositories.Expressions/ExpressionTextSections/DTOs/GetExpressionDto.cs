namespace ExpressedRealms.Repositories.Expressions.ExpressionTextSections.DTOs;

public class GetExpressionTextSectionDto
{
    public int Id { get; init; }
    public string Name { get; set; } = null!;
    public string ShortDescription { get; set; } = null!;
    public string NavMenuImage { get; set; } = null!;
    public PublishTypes PublishStatus { get; set; }
    public List<KeyValuePair<int, string>> PublishTypes { get; set; } = null!;
}
