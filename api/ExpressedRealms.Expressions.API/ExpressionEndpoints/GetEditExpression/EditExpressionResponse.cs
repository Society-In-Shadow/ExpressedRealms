using ExpressedRealms.Expressions.Repository.Expressions;
using ExpressedRealms.Expressions.Repository.Expressions.DTOs;

namespace ExpressedRealms.Expressions.API.ExpressionEndpoints.GetEditExpression;

public class EditExpressionResponse(GetExpressionDto dto)
{
    public int Id { get; init; } = dto.Id;
    public string Name { get; set; } = dto.Name;
    public string ShortDescription { get; set; } = dto.ShortDescription;
    public string NavMenuImage { get; set; } = dto.NavMenuImage;
    public PublishTypes PublishStatus { get; set; } = dto.PublishStatus;
    public List<PublishTypeDto> PublishTypes { get; set; } =
        dto.PublishTypes.Select(x => new PublishTypeDto(x.Key, x.Value)).ToList();
}
