using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Expressions.UseCases.ExpressionTextSections.GetExpressionBooklet;

public interface IGetExpressionBookletUseCase
    : IGenericUseCase<Result<MemoryStream>, GetExpressionBookletModel> { }
