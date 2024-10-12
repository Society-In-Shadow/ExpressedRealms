namespace ExpressedRealms.Repositories.Expressions.Expressions.DTOs;

public class GetExpressionDto
{
    public int Id { get; init; }
    public string Name { get; set; }
    public string ShortDescription { get; set; }
    public string NavMenuImage { get; set; }
    public PublishTypes PublishStatus { get; set; }
    public List<KeyValuePair<int, string>> PublishTypes { get; set; }
}