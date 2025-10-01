using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Expressions.UseCases.StatModifiers.GetModifierTypes;

public interface IGetModifierTypesUseCase
    : IGenericUseCase<Result<List<ModifierTypesReturnModel>>> { }
