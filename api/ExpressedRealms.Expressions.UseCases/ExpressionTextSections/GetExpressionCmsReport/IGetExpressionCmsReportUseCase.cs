using ExpressedRealms.Expressions.UseCases.ExpressionTextSections.DeleteTextSection;
using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Expressions.UseCases.ExpressionTextSections.GetExpressionCmsReport;

public interface IGetExpressionCmsReportUseCase : IGenericUseCase<Result<MemoryStream>, GetExpressionCmsReportModel> { }
