using ExpressedRealms.Repositories.Expressions.Expressions;
using ExpressedRealms.Repositories.Expressions.Expressions.DTOs;
using ExpressedRealms.Server.EndPoints.ExpressionEndpoints.DTOs;

namespace ExpressedRealms.Server.EndPoints.ExpressionEndpoints.Responses;

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
