using ExpressedRealms.Expressions.Repository.Expressions.DTOs;
using Slugify;

namespace ExpressedRealms.Server.EndPoints.NavigationEndpoints.DTOs;

public class ExpressionMenuItem
{
    public ExpressionMenuItem() { }

    public ExpressionMenuItem(ExpressionNavigationMenuItem expressionNavigationMenuItem)
    {
        Id = expressionNavigationMenuItem.Id;
        Name = expressionNavigationMenuItem.Name;
        ShortDescription = expressionNavigationMenuItem.ShortDescription;
        NavMenuImage = expressionNavigationMenuItem.NavMenuImage;
        StatusId = (int)expressionNavigationMenuItem.PublishStatusId;
        StatusName = expressionNavigationMenuItem.PublishStatusName;
        var helper = new SlugHelper();
        Slug = helper.GenerateSlug(expressionNavigationMenuItem.Name);
        ExpressionTypeId = expressionNavigationMenuItem.ExpressionTypeId;
    }

    public int ExpressionTypeId { get; set; }
    public int Id { get; init; }
    public string Name { get; init; }
    public string ShortDescription { get; init; }
    public string NavMenuImage { get; init; }
    public string? StatusName { get; init; }
    public int StatusId { get; init; }
    public string Slug { get; init; }
}
