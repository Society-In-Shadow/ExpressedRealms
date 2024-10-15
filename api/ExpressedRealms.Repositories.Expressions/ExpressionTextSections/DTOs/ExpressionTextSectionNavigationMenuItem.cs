namespace ExpressedRealms.Repositories.Expressions.ExpressionTextSections.DTOs;

public class ExpressionTextSectionNavigationMenuItem
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string ShortDescription { get; set; } = null!;
    public string NavMenuImage { get; set; } = null!;
    public string PublishStatusName { get; set; } = null!;
    public PublishTypes PublishStatusId { get; set; }
}
