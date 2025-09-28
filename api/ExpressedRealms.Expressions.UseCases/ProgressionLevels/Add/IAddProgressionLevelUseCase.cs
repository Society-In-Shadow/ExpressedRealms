using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Expressions.UseCases.ProgressionLevels.Add;

public interface IAddProgressionLevelUseCase
    : IGenericUseCase<Result<int>, AddProgressionLevelModel> { }
