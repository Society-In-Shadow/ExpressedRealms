using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Expressions.UseCases.StatModifiers.GetModifiers;

public interface IGetModifiersUseCase
    : IGenericUseCase<Result<List<StatMappingReturnModel>>, GetModifiersModel> { }
