using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Admin.UseCases.GetPlayer;

public interface IGetPlayerUseCase : IGenericUseCase<Result<PlayerBasicInfoReturnModel>, GetPlayerModel> { }
