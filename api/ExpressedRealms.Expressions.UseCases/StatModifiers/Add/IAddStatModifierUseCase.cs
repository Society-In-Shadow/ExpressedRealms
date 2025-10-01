using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Expressions.UseCases.StatModifiers.Add;

public interface IAddStatModifierUseCase
    : IGenericUseCase<Result<ReturnIds>, AddStatModifierModel> { }
