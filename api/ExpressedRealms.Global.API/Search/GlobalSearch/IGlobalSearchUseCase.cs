using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Global.API.Search.GlobalSearch;

public interface IGlobalSearchUseCase
    : IGenericUseCase<Result<List<GlobalSearchUseCaseReturnModel>>, GlobalSearchUseCaseModel> { }
