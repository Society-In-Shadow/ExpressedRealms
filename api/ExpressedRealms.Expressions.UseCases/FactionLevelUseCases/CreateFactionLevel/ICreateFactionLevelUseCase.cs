using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Expressions.UseCases.FactionLevelUseCases.CreateFactionLevel;

public interface ICreateFactionLevelUseCase
    : IGenericUseCase<Result<int>, CreateFactionLevelModel> { }
