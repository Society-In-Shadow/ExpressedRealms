using ExpressedRealms.Repositories.Expressions.Expressions.DTOs;
using ExpressedRealms.Repositories.Expressions.ExpressionTextSections.DTOs;
using FluentResults;
using GetExpressionDto = ExpressedRealms.Repositories.Expressions.Expressions.DTOs.GetExpressionDto;

namespace ExpressedRealms.Repositories.Expressions.ExpressionTextSections;

public interface IExpressionTextSectionRepository
{
    Task<Result<int>> CreateExpressionAsync(CreateExpressionDto dto);
    Task<Result<int>> EditExpressionAsync(EditExpressionDto dto);
    Task<Result> DeleteExpressionAsync(int id);
    Task<Result<List<ExpressionTextSectionNavigationMenuItem>>> GetNavigationMenuItems();
    Task<Result<GetExpressionTextSectionDto>> GetExpression(int expressionId);
}
