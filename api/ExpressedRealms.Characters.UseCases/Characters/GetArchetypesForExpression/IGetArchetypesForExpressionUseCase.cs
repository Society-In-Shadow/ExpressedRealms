using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Characters.UseCases.Characters.GetArchetypesForExpression;

public interface IGetArchetypesForExpressionUseCase
    : IGenericUseCase<Result<GetArchetypesForExpressionDto>, GetArchetypesForExpressionModel> { }
