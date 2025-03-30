using ExpressedRealms.Repositories.Expressions.Expressions.DTOs;
using ExpressedRealms.Repositories.Expressions.ExpressionTextSections.DTOs;
using FluentResults;

namespace ExpressedRealms.Repositories.Expressions.ExpressionTextSections;

public interface IExpressionTextSectionRepository
{
    Task<Result<int>> CreateExpressionTextSectionAsync(CreateExpressionTextSectionDto dto);
    Task<Result<int>> EditExpressionTextSectionAsync(EditExpressionTextSectionDto dto);
    Task<Result> DeleteExpressionTextSectionAsync(int expressionId, int id);
    Task<Result<GetExpressionTextSectionDto>> GetExpressionTextSection(int sectionId);
    Task<List<ExpressionSectionDto>> GetExpressionTextSections(int expressionId);
    Task<Result<ExpressionTextSectionOptions>> GetExpressionTextSectionOptions(
        GetExpressionTestSectionOptionsDto optionsDto
    );
    Task<Result<int>> GetExpressionId(string expressionName);
    Task<Result> UpdateSectionHierarchyAndSorting(EditExpressionHierarchyDto dto);
    Task<ExpressionSectionDto> GetExpressionSection(int expressionId);
}
