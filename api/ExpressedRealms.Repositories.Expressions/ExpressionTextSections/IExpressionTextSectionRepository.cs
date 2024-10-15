using ExpressedRealms.Repositories.Expressions.Expressions.DTOs;
using ExpressedRealms.Repositories.Expressions.ExpressionTextSections.DTOs;
using FluentResults;

namespace ExpressedRealms.Repositories.Expressions.ExpressionTextSections;

public interface IExpressionTextSectionRepository
{
    Task<Result<int>> CreateExpressionTextSectionAsync(CreateExpressionTextSectionDto dto);
    Task<Result<int>> EditExpressionTextSectionAsync(EditExpressionDto dto);
    Task<Result> DeleteExpressionTextSectionAsync(int id);
    Task<Result<GetExpressionTextSectionDto>> GetExpressionTextSection(int expressionId);
    Task<List<ExpressionSectionDto>> GetExpressionTextSections(int expressionId);
}
