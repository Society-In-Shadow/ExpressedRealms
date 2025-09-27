using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Expressions.UseCases.ProgressionPaths.GetPathsAndLevels;

public interface IGetPathsAndLevelsUseCase
    : IGenericUseCase<Result<List<ProgressionPathReturnModel>>, GetPathsAndLevelsModel> { }
