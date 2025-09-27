using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Expressions.UseCases.ProgressionPaths.Add;

public interface IAddProgressionPathUseCase
    : IGenericUseCase<Result<int>, AddProgressionPathModel> { }
